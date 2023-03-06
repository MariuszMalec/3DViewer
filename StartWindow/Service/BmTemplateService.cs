using System.IO;

namespace StartWindow.Service
{
    public static class BmTemplateService
    {
        public static string GetBMXmlFromBmTemplate(string bmTemplate)
        {
            //szukanie xmla w templacie
            string dirbmtemplatefile = Path.GetDirectoryName(bmTemplate);
            if (Directory.Exists(dirbmtemplatefile))
            {
                string[] files = Directory.GetFiles(dirbmtemplatefile, "*BMD.xml", SearchOption.TopDirectoryOnly);
                foreach (string file in files)
                {
                    string name = Path.GetFileName(file);
                    if (name.Contains("BMD.xml"))
                    {
                        return Path.Combine(dirbmtemplatefile, name);
                    }
                }
            }
            return default;
        }
    }
}
