using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AmazingCloudSearch.Query.Boolean;

namespace AmazingCloudSearch.Query.Boolean
{
    public class StringBooleanCondition : IBooleanCondition
    {
        public string Field { get; set; }
        public string Condition { get; set; }
        protected bool mIsOrCondition { get; set; }

        public StringBooleanCondition(string field, string condition, bool isOrCondition = false)
        {
            Field = field;
            Condition = condition;
            mIsOrCondition = isOrCondition;
        }


       

        public string GetCondictionParam()
        {
            return Field + "%3A" + "'" + Condition + "'";
        }

		public bool IsOrCondition()
		{
			return mIsOrCondition;
		}
        public void IsOrCondition(bool val)
        {
            mIsOrCondition = val;
        }

		public bool IsList()
		{
			return false;
		}
    }
}