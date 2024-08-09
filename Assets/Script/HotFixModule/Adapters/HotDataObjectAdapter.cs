using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using mana.CoreModule;
using mana.CoreModule.HotFix;
using System;

public class HotDataObjectAdapter : CrossBindingAdaptor
{
    public override Type BaseCLRType
    {
        get
        {
            return typeof(HotDataObject);
        }
    }

    public override Type AdaptorType
    {
        get
        {
            return typeof(Adaptor);
        }
    }

    public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
    {
        return new Adaptor(appdomain, instance);
    }

    public class Adaptor : HotDataObject, CrossBindingAdaptorType
    {
        private ILTypeInstance instance;
        ILRuntime.Runtime.Enviorment.AppDomain appdomain;

        IMethod mEncode;
        bool mEncodeGot;

        IMethod mDecode;
        bool mDecodeGot;

        IMethod mReleaseToCache;
        bool mReleaseToCacheGot;

        IMethod mClear;
        bool mClearGot;

        IMethod mToFormatString;
        bool mToFormatStringGot;

        IMethod mTypeCode;
        bool mTypeCodeGot;

        readonly object[] param1 = new object[1];

        public ILTypeInstance ILInstance => instance;

        public Adaptor()
        {
        }

        public Adaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            this.appdomain = appdomain;
            this.instance = instance;
        }

        public void Encode(IWritableBuffer bw)
        {
            if (!mEncodeGot)
            {
                mEncodeGot = true;
                mEncode = instance.Type.GetMethod("Encode", 1);
            }
            if (mEncode != null)
            {
                param1[0] = bw;
                appdomain.Invoke(mEncode, instance, param1);
                param1[0] = null;
            }
        }

        public void Decode(IReadableBuffer br)
        {
            if (!mDecodeGot)
            {
                mDecodeGot = true;
                mDecode = instance.Type.GetMethod("Decode", 1);

            }
            if (mDecode != null)
            {
                param1[0] = br;
                appdomain.Invoke(mDecode, instance, param1);
                param1[0] = null;
            }
        }

        public void ReleaseToCache()
        {
            if (!mReleaseToCacheGot)
            {
                mReleaseToCache = instance.Type.GetMethod("ReleaseToCache", 0);
                mReleaseToCacheGot = true;
            }
            if (mReleaseToCache != null)
            {
                appdomain.Invoke(mReleaseToCache, instance, null);
            }
        }

        public void Clear()
        {
            if (!mClearGot)
            {
                mClear = instance.Type.GetMethod("Clear", 0);
                mClearGot = true;
            }
            if (mClear != null)
            {
                appdomain.Invoke(mClear, instance, null);
            }
        }

        public string ToFormatString(string nlIndent)
        {
            if (!mToFormatStringGot)
            {
                mToFormatStringGot = true;
                mToFormatString = instance.Type.GetMethod("ToFormatString", 1);
            }
            if (mToFormatString != null)
            {
                param1[0] = nlIndent;
                var res = appdomain.Invoke(mToFormatString, instance, param1) as string;
                param1[0] = null;
                return res;
            }
            else
            {
                return null;
            }
        }

        public override string ToString()
        {
            IMethod m = appdomain.ObjectType.GetMethod("ToString", 0);
            m = instance.Type.GetVirtualMethod(m);
            if (m == null || m is ILMethod)
            {
                return instance.ToString();
            }
            else
            {
                return instance.Type.FullName;
            }
        }

        public int GetTypeCode()
        {
            if (!mTypeCodeGot)
            {
                mTypeCodeGot = true;
                mTypeCode = instance.Type.GetMethod("GetTypeCode", 0);
            }
            if (mTypeCode != null)
            {
                var res = (int)appdomain.Invoke(mTypeCode, instance, null);
                return res;
            }
            else
            {
                return 0;
            }
        }
    }

}