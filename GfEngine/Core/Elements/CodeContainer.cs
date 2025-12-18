namespace GfEngine.Core;
public class CodeContainer : GameElement
{
    public int this[string code] => GetAmount(code);
    private Dictionary<string, int> _codeMap;
    public CodeContainer()
    {
        _codeMap = [];
    }
    public void Add(string code, int amount=1)
    {
        if(amount <= 0) throw new Exception("amount must be bigger than 0.");
        if(!_codeMap.ContainsKey(code))
        {
            _codeMap[code] = 0;
        }
        _codeMap[code] += amount;
    }
    public void Remove(string code, int amount=1)
    {
        if(amount <= 0) throw new Exception("amount must be bigger than 0.");
        if(_codeMap.TryGetValue(code, out int count))
        {
            if(count <= amount) _codeMap.Remove(code);
            else _codeMap[code] -= amount;
        }
    }
    public bool Contains(string code, int amount=1)
    {
        if(_codeMap.TryGetValue(code, out int count))
        {
            if(count >= amount) return true;
        }
        return false;
    }
    public int GetAmount(string code)
    {
        if(_codeMap.TryGetValue(code, out int count))
        {
            return count;
        }
        return 0;
    }
    public List<string> GetAllCodes()
    {
        return [.._codeMap.Keys.ToList()];
    }
}