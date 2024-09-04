[System.Serializable]
public class Section
{
    public string sectionname;
    public bool isEndSection;

    public Section(string sectionnam, bool isEndSection)
    {
        this.sectionname = sectionnam;
        this.isEndSection = isEndSection;
    }
}
