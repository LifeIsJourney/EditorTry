using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Net;

public class localizationEditor : EditorWindow
{
    Vector2 scrollPosition;

    SystemLanguage systemLanguage;
    List<SelectedLanguages> userLanguage;
    int userCurrentSelectedLang = 0;
    static EditorWindow w;
    [MenuItem("Window/Test/Local")]
    static void Open()
    {
        w = GetWindow(typeof(localizationEditor));
        w.minSize = new Vector2(600, 520);

        w.Show();
    }

    private void OnEnable()
    {
        userLanguage = new List<SelectedLanguages>();
    }
    bool showLanguage = false;
    private void OnGUI()
    {
        GUIStyle guiStyle = new GUIStyle();
        guiStyle.fontStyle = FontStyle.Bold;
        EditorGUILayout.LabelField("Active Lanuguage", guiStyle);
        GUILayout.BeginVertical(EditorStyles.helpBox);

        if (!showLanguage)
        {
            if (GUILayout.Button("Show Language"))
            {
                showLanguage = true;
            }
        }
        else
        {
            if (GUILayout.Button("Hide Language"))
            {
                showLanguage = false;
            }
        }
        if (showLanguage)
        {
            DrawAddLanguageOption();
        }
        DrawGameText();
        GUILayout.EndVertical();
    }
    void DrawAddLanguageOption()
    {
       // GUILayout.BeginVertical(EditorStyles.helpBox);
        foreach (SelectedLanguages item in userLanguage)
        {
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            GUILayout.Label(item.language.ToString());
            if (GUILayout.Button("Remove",GUILayout.MaxWidth(100)))
            {
                userLanguage.Remove(item);
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.BeginHorizontal();
        GUILayout.Label("User Languages :");
        string[] the_array = userLanguage.Select(i => i.ToString()).ToArray();
        userCurrentSelectedLang = EditorGUILayout.Popup(userCurrentSelectedLang,the_array, GUILayout.MinWidth(100));
        GUILayout.EndHorizontal();
        systemLanguage = (SystemLanguage)EditorGUILayout.EnumPopup("New Language :", systemLanguage);
        //if (isMainLanguageEng)
        //{
        //    if(userLanguage[0] != SystemLanguage.English)
        //          userLanguage.Add(SystemLanguage.English);
        //    isMainLanguageEng = false;
        //}
        // GUILayout.EndVertical();
        if (GUILayout.Button("Add Language"))
        {
            userLanguage.Add(new SelectedLanguages(systemLanguage));
        }
    }
    bool show = false,isMainLanguageEng=true;
    string mainLableText,mainText,translatedText;
    void DrawGameText()
    {
        GUILayout.Space(20);
        GUIStyle gUIStyle = new GUIStyle(EditorStyles.helpBox);
        isMainLanguageEng = GUILayout.Toggle(isMainLanguageEng, "Is Main Language Eng");
        GUILayout.Label("Game Text :");
        GUILayout.BeginVertical(EditorStyles.helpBox);
        show = (EditorGUILayout.Foldout(show, mainLableText, true));
       if(show) {
            GUILayout.BeginHorizontal();
            GUILayout.Label("ID 0");
            mainLableText = GUILayout.TextField(mainLableText, GUILayout.MinWidth(250));
            GUILayout.Button("Remove",GUILayout.Width(100));
            GUILayout.EndHorizontal();
            for (int i = 0; i < userLanguage.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(userLanguage[i].language.ToString());
                if(i == userCurrentSelectedLang)
                {
                    mainText = GUILayout.TextField(mainText, GUILayout.MinWidth(250));
                    userLanguage[i].value = mainText;
                    GUILayout.Button("Enter Text", GUILayout.Width(100));
                }
                else
                {
                    GUILayout.Label(userLanguage[i].value, GUILayout.MinWidth(250));
                   if( GUILayout.Button("Translate", GUILayout.Width(100)))
                    {
                        userLanguage[i].value = GetLocalizedString(userLanguage[0].value, userLanguage[i].language);
                        Debug.Log(userLanguage[i]);
                    }
                }
                GUILayout.EndHorizontal();
            }
          
       }
     
        GUILayout.EndVertical();
        if (GUILayout.Button("adddd something"))
        {
            userLanguage.Add(new SelectedLanguages(SystemLanguage.English));
            userLanguage.Add(new SelectedLanguages(SystemLanguage.German));
            userLanguage.Add(new SelectedLanguages(SystemLanguage.Hungarian));
        }
    }

    string GetLocalizedString(string value, SystemLanguage toConvertInThisLanguage)
    {
        string x="";
        string url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl="
            + GetLanguageCode(userLanguage[userCurrentSelectedLang].language) + "&tl=" + GetLanguageCode(toConvertInThisLanguage) + "&dt=t&q=" + (value);
        Debug.Log(url);
        var json = new WebClient().DownloadString(url);
        string st = json.Substring(3, json.IndexOf(value)+value.Length-1);
        Debug.Log(st);
        string[] val = st.Split(',');
        Debug.Log(val[0]);
        return val[0];
    }
    public static string GetLanguageCode(SystemLanguage language)
    {
        switch (language)
        {
            case SystemLanguage.Afrikaans:
                return "af";
            case SystemLanguage.Italian:
                return "it";
            case SystemLanguage.Arabic:
                return "ar";
            case SystemLanguage.Japanese:
                return "ja";
            case SystemLanguage.Basque:
                return "eu";
            case SystemLanguage.Korean:
                return "ko";
            case SystemLanguage.Belarusian:
                return "be";
            case SystemLanguage.Latvian:
                return "lv";
            case SystemLanguage.Bulgarian:
                return "bg";
            case SystemLanguage.Lithuanian:
                return "lt";
            case SystemLanguage.Catalan:
                return "ca";
            case SystemLanguage.ChineseSimplified:
            case SystemLanguage.Chinese:
                return "zh-CN";
            case SystemLanguage.ChineseTraditional:
                return "zh-TW";
            case SystemLanguage.SerboCroatian:
                return "hr";
            case SystemLanguage.Norwegian:
                return "no";
            case SystemLanguage.Czech:
                return "cs";
            case SystemLanguage.Danish:
                return "da";
            case SystemLanguage.Polish:
                return "pl";
            case SystemLanguage.Dutch:
                return "nl";
            case SystemLanguage.Portuguese:
                return "pt";
            case SystemLanguage.English:
                return "en";
            case SystemLanguage.Romanian:
                return "ro";
            case SystemLanguage.Russian:
                return "ru";
            case SystemLanguage.Estonian:
                return "et";
            case SystemLanguage.Slovak:
                return "sk";
            case SystemLanguage.Finnish:
                return "fi";
            case SystemLanguage.Slovenian:
                return "sl";
            case SystemLanguage.French:
                return "fr";
            case SystemLanguage.Spanish:
                return "es";
            case SystemLanguage.Swedish:
                return "sv";
            case SystemLanguage.German:
                return "de";
            case SystemLanguage.Greek:
                return "el";
            case SystemLanguage.Thai:
                return "th";
            case SystemLanguage.Turkish:
                return "tr";
            case SystemLanguage.Hebrew:
                return "iw";
            case SystemLanguage.Ukrainian:
                return "uk";
            case SystemLanguage.Hungarian:
                return "hu";
            case SystemLanguage.Vietnamese:
                return "vi";
            case SystemLanguage.Icelandic:
                return "is";
            case SystemLanguage.Indonesian:
                return "id";
        }
        return "auto";
    }
}

public class SelectedLanguages
{
   public string value;
   public SystemLanguage language;

    public  SelectedLanguages(SystemLanguage _lang,string _val="")
    {
        value = _val; language = _lang;
    }
}
