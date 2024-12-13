namespace Backend.Tests.Mocks;
public class MockUserDataReader : IDataReader
{
    private readonly IEnumerator<object[]> _enumerator;
    private object[] _current;

    public MockUserDataReader(IEnumerable<object[]> data)
    {
        _enumerator = data.GetEnumerator();
        _current = Array.Empty<object>();
    }

    public bool Read()
    {
        if (_enumerator.MoveNext())
        {
            _current = _enumerator.Current;
            return true;
        }
        return false;
    }

    public object this[int i] => _current[i];

    public object this[string name] => throw new NotImplementedException();

    public int FieldCount => _current.Length;

    public void Dispose() => _enumerator.Dispose();

    public bool NextResult() => false;

    public void Close() { }

    public bool IsClosed => false;

    public int RecordsAffected => 1;

    public string GetName(int i) => throw new NotImplementedException();

    public string GetDataTypeName(int i) => throw new NotImplementedException();

    public Type GetFieldType(int i) => throw new NotImplementedException();

    public object GetValue(int i) => _current[i];

    public int GetValues(object[] values)
    {
        _current.CopyTo(values, 0);
        return _current.Length;
    }

    public int GetOrdinal(string name) => throw new NotImplementedException();

    public bool GetBoolean(int i) => (bool)_current[i];

    public byte GetByte(int i) => throw new NotImplementedException();

    public long GetBytes(int i, long fieldOffset, byte[]? buffer, int bufferoffset, int length) => throw new NotImplementedException();

    public char GetChar(int i) => throw new NotImplementedException();

    public long GetChars(int i, long fieldoffset, char[]? buffer, int bufferoffset, int length) => throw new NotImplementedException();

    public Guid GetGuid(int i) => throw new NotImplementedException();

    public short GetInt16(int i) => throw new NotImplementedException();

    public int GetInt32(int i) => (int)_current[i];

    public long GetInt64(int i) => throw new NotImplementedException();

    public float GetFloat(int i) => throw new NotImplementedException();

    public double GetDouble(int i) => throw new NotImplementedException();

    public string GetString(int i) => _current[i].ToString()!;

    public decimal GetDecimal(int i) => throw new NotImplementedException();

    public DateTime GetDateTime(int i) => throw new NotImplementedException();

    public IDataReader GetData(int i) => throw new NotImplementedException();

    public bool IsDBNull(int i) => _current[i] == null;

    public DataTable? GetSchemaTable()
    {
        throw new NotImplementedException();
    }

    public int Depth => 0;
}