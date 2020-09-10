﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Shimakaze.Struct.Csf.Helper
{
    public static class CsfHeadHelper
    {
        // 标准CSF文件标识符
        public static readonly byte[] CSF_FLAG = { 32, 70, 83, 67, };

        public const int CSF_VERSION_2 = 2;
        public const int CSF_VERSION_3 = 3;

        // Ares 某版本开始的特有的语言
        public const int Languages_Auto = -1;
        public const int Languages_de = 2;
        public const int Languages_en_UK = 1;
        public const int Languages_en_US = 0;
        public const int Languages_es = 4;
        public const int Languages_fr = 3;
        public const int Languages_it = 5;
        public const int Languages_ja = 6;
        // 不明语言
        public const int Languages_Jabberwockie = 7;
        public const int Languages_ko = 8;

        public const int Languages_zh = 9;

        public static CsfHead Create(int version, int labelCount, int stringCount, int unknown, int lang)
        {
            CsfHead head;
            head.Version = version;
            head.LabelCount = labelCount;
            head.StringCount = stringCount;
            head.Unknown = unknown;
            head.Language = lang;
            return head;
        }
        public static async Task<CsfHead> DeserializeAsync(Stream stream)
        {
            if (!(await stream.ReadAsync(4)).SequenceEqual(CSF_FLAG)) throw new FormatException("Unknown File Format: Unknown Header");
            return Create(
                BitConverter.ToInt32(await stream.ReadAsync(4), 0),
                BitConverter.ToInt32(await stream.ReadAsync(4), 0),
                BitConverter.ToInt32(await stream.ReadAsync(4), 0),
                BitConverter.ToInt32(await stream.ReadAsync(4), 0),
                BitConverter.ToInt32(await stream.ReadAsync(4), 0));
        }
        public static async Task SerializeAsync(this CsfHead head, Stream stream)
        {
            await stream.WriteAsync(CSF_FLAG);
            await stream.WriteAsync(BitConverter.GetBytes(head.Version));
            await stream.WriteAsync(BitConverter.GetBytes(head.LabelCount));
            await stream.WriteAsync(BitConverter.GetBytes(head.StringCount));
            await stream.WriteAsync(BitConverter.GetBytes(head.Unknown));
            await stream.WriteAsync(BitConverter.GetBytes(head.Language));
        }

    }
}
