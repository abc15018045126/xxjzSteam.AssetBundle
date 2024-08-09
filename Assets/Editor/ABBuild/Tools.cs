using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Assets.Editor.ABBuild
{
    public static class Tools
    {
        public static string GetRelativePathWithoutExtension(string path, string root)
        {
            var rootIndex = path.LastIndexOf(root);
            string ret = null;
            if (rootIndex >= 0)
            {
                ret = path.Substring(rootIndex + root.Length);
            }
            else
            {
                ret = root;
            }
            if (ret[0] == '/' || ret[0] == '\\')
            {
                ret = ret.Substring(1);
            }
            return Path.ChangeExtension(ret, null);
        }

        public static void Add(Dictionary<string, HashSet<string>> dic, string[] keys, string value)
        {
            HashSet<string> find;
            foreach (var k in keys)
            {
                if (!dic.TryGetValue(k, out find))
                {
                    find = new HashSet<string>();
                    dic.Add(k, find);
                }
                find.Add(value);
            }
        }


        public static string CalculateMD5Hash(string content)
        {
            // step 1, calculate MD5 hash from input
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            var inputBytes = System.Text.Encoding.ASCII.GetBytes(content);
            var hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public static string ToHexString(byte[] data)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                byte d = data[i];
                sb.Append(d.ToString("x2"));
            }
            return sb.ToString();
        }

        public static string CalculateMD5(Stream stream)
        {
            var md5Hasher = System.Security.Cryptography.MD5.Create();
            md5Hasher.ComputeHash(stream);
            return ToHexString(md5Hasher.Hash);
        }

        public static string CalculateMD5(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                return CalculateMD5(ms);
            }
        }

        public static string CalculateMD5(string text)
        {
            using (var ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(text)))
            {
                return CalculateMD5(ms);
            }
        }

        public static string HashSetToString<T>(HashSet<T> hs)
        {
            var sb = new System.Text.StringBuilder();
            foreach (var _ref in hs)
            {
                if (sb.Length > 0)
                {
                    sb.Append('-');
                }
                sb.Append(_ref);
            }
            return sb.ToString();
        }


        public static void ClearDirectory(string file)
        {
            try
            {
                //去除文件夹和子文件的只读属性
                //去除文件夹的只读属性
                DirectoryInfo fileInfo = new DirectoryInfo(file);
                fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;

                //去除文件的只读属性
                File.SetAttributes(file, FileAttributes.Normal);

                //判断文件夹是否还存在
                if (Directory.Exists(file))
                {
                    foreach (string f in Directory.GetFileSystemEntries(file))
                    {
                        if (File.Exists(f))
                        {
                            //如果有子文件删除文件
                            File.Delete(f);

                        }
                        else
                        {
                            Directory.Delete(f);
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                Debug.LogError(ex);
            }
        }
    }
}
