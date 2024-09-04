using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UserDataManager : MonoBehaviour
{
    public UserData userData = new UserData();
    // Mutexのインスタンスを作成
    private static readonly Mutex mutex = new Mutex(false, "JsonFileMutex");
    string filePath;                            // jsonファイルのパス
    string fileName = "UserData.json";              // jsonファイル名

    private void Awake()
    {
        // パス名取得
        filePath = Application.dataPath + "/" + fileName;

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "{}");
        }
    }

    // ユーザーデータをJSONファイルから読み込む
    public async Task LoadUserData()
    {
        // ファイルが存在するかをチェック
        if (!File.Exists(filePath))
        {
            Debug.LogError("ファイルが見つかりません: " + filePath);
            return;
        }

        // ロックを取得
        mutex.WaitOne();

        while (true)
        {
            try
            {
                // ファイルからデータを読み込む
                string json = File.ReadAllText(filePath, encoding:System.Text.Encoding.UTF8);
                userData = JsonUtility.FromJson<UserData>(json);
                break;
            }
            catch
            {
                await Task.Delay(200);
            }
        }

        // ロックを解除
        mutex.ReleaseMutex();
        await Task.Yield();
    }


    // ユーザーデータをJSONファイルに保存する
    public async Task SaveUserData()
    {
        string json = JsonUtility.ToJson(userData, true);
        // ロックを取得
        mutex.WaitOne();

        while (true)
        {
            try
            {
                // ファイルにデータを書き込む
                Console.WriteLine("書き込み開始: " + json);
                File.WriteAllText(filePath, json, encoding: System.Text.Encoding.UTF8);
                Console.WriteLine("書き込み完了: " + json);
                break;
            }
            catch
            {
                await Task.Delay(200);
            }

        }

        // ロックを解除
        mutex.ReleaseMutex();
        await Task.Yield();
    }

    // すでにそのユーザー名が存在するか
    public async Task<bool> IsExistUserName(string username)
    {
        // ユーザー名の重複チェック
        for(int i = 0; i < userData.users.Count; i++)
        {
            if (userData.users[i].username == username)
            {
                await Task.Yield();
                return true;
            }
        }

        await Task.Yield();
        return false;
    }

    // すでにそのパスワードが存在するか
    public async Task<bool> IsCorrectPassWord(string username, string password)
    {
        // ユーザー名の重複チェック
        for (int i = 0; i < userData.users.Count; i++)
        {
            if (userData.users[i].username == username)
            {
                if(userData.users[i].password == password)
                {
                    return true;
                }
            }
        }

        await Task.Yield();
        return false;
    }

    //ユーザーを登録する
    public async Task RegisterUser(string username, string password)
    {
        // 新しいユーザーを追加
        userData.users.Add(new User(username, password, 0));
        await SaveUserData();
    }

    // スコアを更新する
    public async Task UpdateScore(string username, int score)
    {
        //セクションが終了していたら更新しない
        if (SectionDataManager.Instance.IsEndCurrentSection()) return;

        for (int i = 0; i < userData.users.Count; i++)
        {
            if (userData.users[i].username == username)
            {
                //現在のスコアよりも高かったらスコアをアップデート
                if (userData.users[i].score < score)
                {
                    await LoadUserData();
                    userData.users[i].score = score;
                    await SaveUserData();
                    return;
                }
            }
        }
    }

    // ランキングの取得
    public List<User> GetRankingList()
    {
        // スコアが0以上のユーザーのみをフィルタリングし、スコア順に並び替え
        var list = userData.users
            .Where(user => user.score > 0)   // スコアが0のユーザーを除外
            .OrderByDescending(user => user.score) // スコアの降順で並び替え
            .ToList(); // Listに変換

        return list;
    }
}
