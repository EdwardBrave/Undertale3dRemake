using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public interface IChangeable{}
    
    public class Changeable<T> : IChangeable
    {
        private T _value;
        private bool _isChanged;

        public bool IsChanged => _isChanged;
        
        public T Value
        {
            get => _value;
            set
            {
                if (EqualityComparer<T>.Default.Equals(_value, value))
                    return;
                _value = value;
                _isChanged = true;
            }
        }
        
        public Changeable() { }

        public Changeable(T value)
        {
            _value = value;
            _isChanged = true;
        }

        public void SetQuietly(T value) => _value = value;

        public bool CheckIfChanged()
        {
            if (!_isChanged) 
                return false;
            _isChanged = false;
            return true;
        }
    }
    
    public static class ChangeableListExtension
    {
        public static bool IsChanged<T>(this Dictionary<string, Changeable<T>> self) => 
            self.Values.Any(item => item.IsChanged);

        public static Dictionary<string, T> Changes<T>(this Dictionary<string, Changeable<T>> self) =>
            self.Where(pair => pair.Value.CheckIfChanged())
                .ToDictionary(pair => pair.Key, pair => pair.Value.Value);
    }
}