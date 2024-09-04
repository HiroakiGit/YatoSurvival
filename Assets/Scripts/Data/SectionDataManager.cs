using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class SectionDataManager : MonoBehaviour
{
    public static SectionDataManager Instance;
    public SectionData sectionData = new SectionData();
    // Mutexのインスタンスを作成
    private static readonly Mutex mutex = new Mutex(false, "JsonFileMutex");
    string filePath;                           
    string fileName = "SectionData.json";             

    private void Awake()
    {
        // シングルトンパターンの実装
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // パス名取得
        filePath = Application.dataPath + "/" + fileName;

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "{}");
            // 新しいセクションを追加
            sectionData.section = new Section(string.Empty, false);
            string json = JsonUtility.ToJson(sectionData, true);
            // ファイルに書き込み
            while (true)
            {
                try
                {
                    // ファイルに書き込み
                    using (StreamWriter writer = new StreamWriter(filePath, false, encoding:System.Text.Encoding.UTF8))
                    {
                        writer.Write(json);
                    }

                    break; // 成功したらループを抜ける
                }
                catch
                {
                    System.Threading.Thread.Sleep(100); // 少し待ってから再試行
                }
            }
        }
    }

    //セクションデータを読み込む
    public async Task LoadSectionData()
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
                sectionData = JsonUtility.FromJson<SectionData>(json);
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
    
    //現在のセクションが終わったか
    public bool IsEndCurrentSection()
    {
        if (sectionData == null) return false;

        return sectionData.section.isEndSection;
    }

    //現在のセクションの名前
    public string GetCurrentSectionName()
    {
        if (sectionData == null) return string.Empty;

        return sectionData.section.sectionname;
    }
}
