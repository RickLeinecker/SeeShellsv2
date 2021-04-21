using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SeeShellsV2.Utilities;

namespace SeeShellsV2.Data
{
    public class ExtensionBlock : IExtensionBlock
    {
        public ushort Size
        {
            init => fields["Size"] = value;
            get => fields.GetStructOrDefault<ushort>("Size", 0);
        }

        public ushort ExtensionVersion
        {
            init => fields["ExtensionVersion"] = value;
            get => fields.GetStructOrDefault<ushort>("ExtensionVersion", 0);
        }

        public uint Signature
        {
            init => fields["Signature"] = value;
            get => fields.GetStructOrDefault<uint>("Signature", 0);
        }

        public IReadOnlyDictionary<string, object> Fields
        {
            get => fields;
        }

        public ExtensionBlock() { }

        public ExtensionBlock(byte[] buf, int offset)
        {
            fields["Size"] = BlockHelper.UnpackWord(buf, offset + 0x00);
            fields["ExtensionVersion"] = BlockHelper.UnpackWord(buf, offset + 0x02);
            fields["Signature"] = BlockHelper.UnpackDWord(buf, offset + 0x04);
        }

        protected Dictionary<string, object> fields = new Dictionary<string, object>();
    }
}
