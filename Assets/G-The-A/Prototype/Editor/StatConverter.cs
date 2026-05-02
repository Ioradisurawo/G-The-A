using UnityEditor;
using UnityEngine.UIElements;
using static Stat;
public static class StatConverter
{
    #if UNITY_EDITOR
    [InitializeOnLoadMethod]
    #endif
    public static void RegisterConverters()
    {

        // Create local Converters
        var group = new ConverterGroup("Enum Name to String");

        group.AddConverter((ref NAME value) =>
        {
            switch (value)
            {
                case (NAME)0:
                    return "Maid";
                case (NAME)1:
                    return "Harman Smith";
                case (NAME)2:
                    return "Dan Smith";
                case (NAME)3:
                    return "KAEDE Smith";
                case (NAME)4:
                    return "Garcian Smith";
                default:
                    return "Unknown";
            }
        });

        // Register the converter group in InitializeOnLoadMethod to make it accessible from the UI Builder.
        ConverterGroups.RegisterConverterGroup(group);
    }
}
