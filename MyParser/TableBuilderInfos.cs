using System;
using System.Collections.Generic;
using System.Linq;
using MyLibrary;
using String = MyLibrary.Types.String;

namespace MyParser
{
    [Serializable]
    public class TableBuilderInfos : Dictionary<string, Dictionary<string, TableBuilderInfo>>, IValueable
    {
        public Values ToValues()
        {
            return new Values(this);
        }

        public override string ToString()
        {
            return
                String.Parse(new Transformation().ParseTemplate(new Values(Keys, this.Select(item => item.ToString()))));
        }

        public void Add(TableBuilderInfo builderInfo)
        {
            if (!ContainsKey(builderInfo.TableName.ToString())) Add(builderInfo.TableName.ToString(), new Dictionary<string, TableBuilderInfo>());
            this[builderInfo.TableName.ToString()].Add(builderInfo.FieldName.ToString(), builderInfo);
        }
    }
}