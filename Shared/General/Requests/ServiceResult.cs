using System.Collections.Generic;
using System.Linq;

namespace General.Requests
{
    public class ServiceResult<T>
    {
        public ServiceResult()
        {
            Errors = new List<string>();
            Properties = new Dictionary<string, string>();
        }
        
        public bool Succeeded { get; set; }
        public List<string> Errors { get; protected set; }
        public Dictionary<string, string> Properties { get; protected set; }
        public T Value { get; set; }


        public void AddProperty(string key, string value)
        {
            if(string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value)) return;

            if (Properties.ContainsKey(key))
            {
                Properties[key] = Properties[key] + ", " + value;
            }
            else
            {
                Properties[key] = value;
            }
        }

        public static ServiceResult<T> Error(params string[] errors) 
        {
            return new ServiceResult<T>
            {
                Succeeded = false,
                Errors = errors.ToList()
            };
        }

        public static ServiceResult<T> Success() 
        {
            return new ServiceResult<T>
            {
                Succeeded = true
            };
        }

        public static ServiceResult<T> Success(T value) 
        {
            return new ServiceResult<T>
            {
                Succeeded = true,
                Value = value
            };
        }
    }
}
