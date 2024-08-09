using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Intepreter;
using mana.CoreModule.Network;
using mana.CoreModule.Network.Client;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HotFixImpl : MonoBehaviour, HotFix.IHotFixImpl
{

    #region Singleton Instance

    static HotFixImpl _instance = null;
    public static HotFixImpl Instance
    {
        get
        {
            if (_instance == null)
            {
                var _go = new GameObject("_HotFixImpl");
                _instance = _go.AddComponent<HotFixImpl>();
                GameObject.DontDestroyOnLoad(_go);
            }
            return _instance;
        }
    }

    #endregion

    public ILRuntime.Runtime.Enviorment.AppDomain appdomain { get; private set; } = null;

    public IType HotfixMainEnter { get; private set; } = null;

    #region MonoBehaviour

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogErrorFormat("{0} is Singleton!", GetType());
            GameObject.Destroy(gameObject);
        
        }
    }

    void OnDestroy()
    {
        this.ReleaseAssemblyData();
    }

    #endregion

    #region LoadHotFixAssembly

    System.IO.MemoryStream fs = null;
    System.IO.MemoryStream ps = null;

    void HotFix.IHotFixImpl.LoadHotFixAssembly()
    {
        var dll = ConfigsLoader.Instance.fileProvider.TryGetHotFixScriptFileData("HotCode.dll");
        if (dll == null)
        {
            Debug.LogError("LoadHotFixAssembly dll failed!");
            return;
        }
        else
        {
            fs = new System.IO.MemoryStream(dll);
        }
        var pdb = ConfigsLoader.Instance.fileProvider.TryGetHotFixScriptFileData("HotCode.pdb");
        if (pdb == null)
        {
            Debug.Log("LoadHotFixAssembly pdb failed!");
        }
        else
        {
            ps = new System.IO.MemoryStream(pdb);
        }
        appdomain = new ILRuntime.Runtime.Enviorment.AppDomain();
        appdomain.LoadAssembly(fs, ps, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
        InitializeILRuntime();
        OnHotFixLoaded();
    }

    void ReleaseAssemblyData()
    {
        if (fs != null)
        {
            fs.Close();
        }
        if (ps != null)
        {
            ps.Close();
        }
        fs = null;
        ps = null;
    }

    #endregion

    #region implement GameApp.IGameChannelEventListener

    private readonly HashSet<string> gameChannelDispatches = new HashSet<string>();

    IMethod mGameChannelDispatchMethod = null;


    void HotFix.IHotFixImpl.AddGameChannelDispatch(string msgRoute)
    {
        gameChannelDispatches.Add(msgRoute);
    }

    #endregion

    #region implement GameApp.ISceneChannelEventListener

    private readonly HashSet<string> sceneChannelDispatches = new HashSet<string>();

    IMethod mSceneChannelDispatchMethod = null;


    void HotFix.IHotFixImpl.AddSceneChannelDispatch(string msgRoute)
    {
        sceneChannelDispatches.Add(msgRoute);
    }

    #endregion

    #region HotFix.IHotFixImpl

    private readonly object[] paramCache1 = new object[1];
    private readonly object[] paramCache2 = new object[2];
    private readonly object[] paramCache3 = new object[3];
    private readonly object[] paramCache4 = new object[4];
    private readonly object[] paramCache5 = new object[5];

    public ILTypeInstance CreateHotFixScriptObject(string hotFixClassName, GameObject attach)
    {
        if (appdomain == null || string.IsNullOrWhiteSpace(hotFixClassName))
        {
            return null;
        }
        paramCache1[0] = attach;
        return appdomain.Instantiate(hotFixClassName, paramCache1);
    }

    public IMethod GetHotfixMainMethod(string method, int paramCount)
    {
        return HotfixMainEnter.GetMethod(method, paramCount);
    }

    #endregion

    #region HotFix.IHotFixImpl - InvokeMethod

    private object __InvokeMethod(string type, string method, object instance, object[] p)
    {
        return appdomain.Invoke(type, method, instance, p);
    }


    object HotFix.IHotFixImpl.InvokeMethod(string type, string method, object instance)
    {
        return __InvokeMethod(type, method, instance, null);
    }

    object HotFix.IHotFixImpl.InvokeMethod(string type, string method, object instance, object arg1)
    {
        paramCache1[0] = arg1;
        return __InvokeMethod(type, method, instance, paramCache1);
    }

    object HotFix.IHotFixImpl.InvokeMethod(string type, string method, object instance, object arg1, object arg2)
    {
        paramCache2[0] = arg1;
        paramCache2[1] = arg2;
        return __InvokeMethod(type, method, instance, paramCache2);
    }

    object HotFix.IHotFixImpl.InvokeMethod(string type, string method, object instance, object arg1, object arg2, object arg3)
    {
        paramCache3[0] = arg1;
        paramCache3[1] = arg2;
        paramCache3[2] = arg3;
        return __InvokeMethod(type, method, instance, paramCache3);
    }

    object HotFix.IHotFixImpl.InvokeMethod(string type, string method, object instance, object arg1, object arg2, object arg3, object arg4)
    {
        paramCache4[0] = arg1;
        paramCache4[1] = arg2;
        paramCache4[2] = arg3;
        paramCache4[3] = arg4;
        return __InvokeMethod(type, method, instance, paramCache4);
    }

    object HotFix.IHotFixImpl.InvokeMethod(string type, string method, object instance, object arg1, object arg2, object arg3, object arg4, object arg5)
    {
        paramCache5[0] = arg1;
        paramCache5[1] = arg2;
        paramCache5[2] = arg3;
        paramCache5[3] = arg4;
        paramCache5[4] = arg5;
        return __InvokeMethod(type, method, instance, paramCache5);
    }


    #endregion

    #region HotFix.IHotFixImpl - InvokeMainMethod

    private object __InvokeMainMethod(string method, object[] p)
    {
        if (appdomain == null || HotfixMainEnter == null)
        {
            return null;
        }
        var m = HotfixMainEnter.GetMethod(method, p != null ? p.Length : 0);
        if (m == null)
        {
            return null;
        }
        return appdomain.Invoke(m, null, p);
    }

    object HotFix.IHotFixImpl.GetMainMethodWithoutParam(string methodName)
    {
        if (appdomain == null || HotfixMainEnter == null)
        {
            return null;
        }
        return HotfixMainEnter.GetMethod(methodName, 0);
    }

    void HotFix.IHotFixImpl.InvokeMainMethodWithoutParam(object method)
    {
        if (appdomain == null || HotfixMainEnter == null)
        {
            return;
        }
        if (method is IMethod m)
        {
            appdomain.Invoke(m, null);
        }
    }

    object HotFix.IHotFixImpl.InvokeMainMethod(string method)
    {
        return __InvokeMainMethod(method, null);
    }

    object HotFix.IHotFixImpl.InvokeMainMethod(string method, object arg1)
    {
        paramCache1[0] = arg1;
        return __InvokeMainMethod(method, paramCache1);
    }

    object HotFix.IHotFixImpl.InvokeMainMethod(string method, object arg1, object arg2)
    {
        paramCache2[0] = arg1;
        paramCache2[1] = arg2;
        return __InvokeMainMethod(method, paramCache2);
    }

    object HotFix.IHotFixImpl.InvokeMainMethod(string method, object arg1, object arg2, object arg3)
    {
        paramCache3[0] = arg1;
        paramCache3[1] = arg2;
        paramCache3[2] = arg3;
        return __InvokeMainMethod(method, paramCache3);
    }

    object HotFix.IHotFixImpl.InvokeMainMethod(string method, object arg1, object arg2, object arg3, object arg4)
    {
        paramCache4[0] = arg1;
        paramCache4[1] = arg2;
        paramCache4[2] = arg3;
        paramCache4[3] = arg4;
        return __InvokeMainMethod(method, paramCache4);
    }

    object HotFix.IHotFixImpl.InvokeMainMethod(string method, object arg1, object arg2, object arg3, object arg4, object arg5)
    {
        paramCache5[0] = arg1;
        paramCache5[1] = arg2;
        paramCache5[2] = arg3;
        paramCache5[3] = arg4;
        paramCache5[4] = arg5;
        return __InvokeMainMethod(method, paramCache5);
    }

    #endregion

    void OnHotFixLoaded()
    {
        HotfixMainEnter = appdomain.GetType("HotCode.Main");

        mGameChannelDispatchMethod = HotfixMainEnter.GetMethod("OnGameChannelDispatch", 1);
        if (mGameChannelDispatchMethod == null)
        {
            Debug.LogError("can't find method OnGameChannelDispatch");
        }

        mSceneChannelDispatchMethod = HotfixMainEnter.GetMethod("OnSceneChannelDispatch", 1);
        if (mGameChannelDispatchMethod == null)
        {
            Debug.LogError("can't find method OnSceneChannelDispatch");
        }

        __InvokeMainMethod("OnLoaded", null);
    }

    void InitializeILRuntime()
    {

//#if DEBUG && (UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE)
//        //由于Unity的Profiler接口只允许在主线程使用，为了避免出异常，需要告诉ILRuntime主线程的线程ID才能正确将函数运行耗时报告给Profiler
//        appdomain.UnityMainThreadID = Thread.CurrentThread.ManagedThreadId;
//#endif
//        LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appdomain);
//        ILRuntime.Runtime.Generated.CLRBindings.Initialize(appdomain);

//        appdomain.DelegateManager.RegisterFunctionDelegate<mana.CoreModule.HotFix.HotDataObject>();
//        appdomain.DelegateManager.RegisterMethodDelegate<AssetBundleRequest, object>();
//        appdomain.DelegateManager.RegisterMethodDelegate<ScrollItemsViewItem, System.Object>();
//        appdomain.DelegateManager.RegisterFunctionDelegate<Packet, bool>();
//        appdomain.DelegateManager.RegisterMethodDelegate<RectTransform, int, object>();
//        appdomain.DelegateManager.RegisterMethodDelegate<byte, object>();//        appdomain.DelegateManager.RegisterMethodDelegate<TabView>();//        appdomain.DelegateManager.RegisterFunctionDelegate<global::HotDataObjectAdapter.Adaptor, global::HotDataObjectAdapter.Adaptor, System.Int32>();//        appdomain.DelegateManager.RegisterFunctionDelegate<global::HotDataObjectAdapter.Adaptor, System.Boolean>();
//        appdomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Boolean>();
//        appdomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>();//        appdomain.DelegateManager.RegisterMethodDelegate<System.Int32, System.String, System.Object>();//        appdomain.DelegateManager.RegisterMethodDelegate<System.Boolean>();//        appdomain.DelegateManager.RegisterMethodDelegate<System.String>();//        appdomain.DelegateManager.RegisterMethodDelegate<System.Byte, System.String, System.Object>();//        appdomain.DelegateManager.RegisterMethodDelegate<System.Byte, System.Int64, System.Int64>();//        appdomain.DelegateManager.RegisterMethodDelegate<System.Int64, System.Int32, System.Int32>();//        appdomain.DelegateManager.RegisterMethodDelegate<System.Int32>();//        appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction>((act) =>
//        {
//            return new UnityEngine.Events.UnityAction(() =>
//            {
//                ((Action)act)();
//            });
//        });

//        appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<TabView>>((act) =>
//        {
//            return new UnityEngine.Events.UnityAction<TabView>((arg0) =>
//            {
//                ((Action<TabView>)act)(arg0);
//            });
//        });

//        appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<global::HotDataObjectAdapter.Adaptor>>((act) =>
//        {
//            return new System.Comparison<global::HotDataObjectAdapter.Adaptor>((x, y) =>
//            {
//                return ((Func<global::HotDataObjectAdapter.Adaptor, global::HotDataObjectAdapter.Adaptor, System.Int32>)act)(x, y);
//            });
//        });
//        appdomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<global::HotDataObjectAdapter.Adaptor>>((act) =>
//        {
//            return new System.Predicate<global::HotDataObjectAdapter.Adaptor>((obj) =>
//            {
//                return ((Func<global::HotDataObjectAdapter.Adaptor, System.Boolean>)act)(obj);
//            });
//        });
//        appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>>((act) =>
//        {
//            return new System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>((x, y) =>
//            {
//                return ((Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>)act)(x, y);
//            });
//        });//        appdomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<ILRuntime.Runtime.Intepreter.ILTypeInstance>>((act) =>
//        {
//            return new System.Predicate<ILRuntime.Runtime.Intepreter.ILTypeInstance>((obj) =>
//            {
//                return ((Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Boolean>)act)(obj);
//            });
//        });//        appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.Boolean>>((act) =>
//        {
//            return new UnityEngine.Events.UnityAction<System.Boolean>((arg0) =>
//            {
//                ((Action<System.Boolean>)act)(arg0);
//            });
//        });//        appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.String>>((act) =>
//        {
//            return new UnityEngine.Events.UnityAction<System.String>((arg0) =>
//            {
//                ((Action<System.String>)act)(arg0);
//            });
//        });
//        appdomain.RegisterCrossBindingAdaptor(new HotDataObjectAdapter());
//        appdomain.DelegateManager.RegisterMethodDelegate<global::HotDataObjectAdapter.Adaptor>();
    }
}
