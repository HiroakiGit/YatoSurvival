[System.Serializable]
public class User
{
    public string username;
    public string password;
    public int score = 0;

    public User(string username, string password,int score)
    {
        this.username = username;
        this.password = password;
        this.score = score;
    }
}
