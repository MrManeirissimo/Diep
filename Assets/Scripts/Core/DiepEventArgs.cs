using System.Collections.Generic;

public class DiepEventArgs {
    public DiepEventArgs(Dictionary<string, object> propertiesByName) {
        this.propertiesByName = propertiesByName;
    }
    public DiepEventArgs() {
        propertiesByName = new Dictionary<string, object>();
    }

    Dictionary<string, object> propertiesByName;

    public void AddProp(string key, object value) => propertiesByName.Add(key, value);
    public object Get(string key) => propertiesByName[key];
    public T Get<T>(string key) => (T)propertiesByName[key];
}