using VRCPimaxEyeTracker;
using MelonLoader;
using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using VRC.SDK3.Avatars.ScriptableObjects;

[assembly: MelonInfo(typeof(VRCPimaxEyeTrackerMod), "VRCPimaxEyeTracker", "1.0.0", "Genesis", "https://github.com/NGenesis/VRCPimaxEyeTracker")]
[assembly: MelonGame("VRChat", "VRChat")]

namespace VRCPimaxEyeTracker
{
    public static class DependencyManager
    {
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        public static bool Init() => LoadDLL("VRCPimaxEyeTracker.PimaxEyeTracker", "PimaxEyeTracker.dll");

        private static bool LoadDLL(string resourcePath, string dllName)
        {
            var dllPath = Path.Combine(Path.GetTempPath(), dllName);

            using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath + "." + dllName))
            {
                if (resource == null) return false;

                using (Stream dllFile = File.Create(dllPath))
                {
                    resource.Seek(0, SeekOrigin.Begin);
                    resource.CopyTo(dllFile);
                }
            }

            return LoadLibrary(dllPath) != IntPtr.Zero;
        }
    }

    public static class ExpressionParameterManager
    {
        private static AvatarAnimParamController AnimParamController => VRCPlayer.field_Internal_Static_VRCPlayer_0?.field_Private_AnimatorControllerManager_0?.field_Private_AvatarAnimParamController_0;
        private static AvatarPlayableController PlayableController => AnimParamController?.field_Private_AvatarPlayableController_0;
        private static VRCExpressionParameters.Parameter[] ExpressionParameterDescriptors => VRCPlayer.field_Internal_Static_VRCPlayer_0?.prop_VRCAvatarManager_0?.prop_VRCAvatarDescriptor_0?.expressionParameters?.parameters;
        private static UnhollowerBaseLib.Il2CppReferenceArray<AvatarPlayableController.ObjectNPublicInObInPaInUnique> AnimatorParameters => PlayableController?.field_Private_ArrayOf_ObjectNPublicInObInPaInUnique_0;

        public static void SetParameter(string name, float value, bool prioritise = false)
        {
            if (ExpressionParameterDescriptors == null || PlayableController == null) return;

            for (var i = 0; i < ExpressionParameterDescriptors.Length; ++i)
            {
                var expressionParameter = ExpressionParameterDescriptors[i];
                if (expressionParameter.name == name)
                {
                    switch(expressionParameter.valueType) {
                        case VRCExpressionParameters.ValueType.Bool:
                            value = value <= float.Epsilon ? 0.0f : 1.0f;
                            break;

                        case VRCExpressionParameters.ValueType.Int:
                            value = (int)value;
                            break;
                    }

                    PlayableController?.Method_Public_Boolean_Int32_Single_0(i, value); // Set parameter at index
                    if (prioritise) PlayableController?.Method_Public_Void_Int32_0(i); // Set parameter priority
                    return;
                }
            }
        }

        public static void SetParameter(string name, bool value, bool prioritise = false) => SetParameter(name, value ? 1.0f : 0.0f, prioritise);
        public static void SetParameter(string name, int value, bool prioritise = false) => SetParameter(name, (float)value, prioritise);

        public static bool GetBoolParameter(string paramName)
        {
            if (AnimatorParameters != null && !string.IsNullOrEmpty(paramName))
            {
                foreach (var animatorParameter in AnimatorParameters)
                {
                    VRCExpressionParameters.Parameter expressionParameter = animatorParameter.field_Public_Parameter_0;
                    if (expressionParameter.name == paramName && expressionParameter.valueType == VRCExpressionParameters.ValueType.Bool) return animatorParameter?.field_Public_AvatarParameter_0?.prop_Boolean_0 ?? false;
                }
            }

            return false;
        }

        public static float GetFloatParameter(string paramName)
        {
            if (AnimatorParameters != null && !string.IsNullOrEmpty(paramName))
            {
                foreach (var animatorParameter in AnimatorParameters)
                {
                    VRCExpressionParameters.Parameter expressionParameter = animatorParameter.field_Public_Parameter_0;
                    if (expressionParameter.name == paramName && expressionParameter.valueType == VRCExpressionParameters.ValueType.Float) return animatorParameter?.field_Public_AvatarParameter_0?.prop_Single_0 ?? 0.0f;
                }
            }

            return 0.0f;
        }

        public static int GetIntParameter(string paramName)
        {
            if (AnimatorParameters != null && !string.IsNullOrEmpty(paramName))
            {
                foreach (var animatorParameter in AnimatorParameters)
                {
                    VRCExpressionParameters.Parameter expressionParameter = animatorParameter.field_Public_Parameter_0;
                    if (expressionParameter.name == paramName && expressionParameter.valueType == VRCExpressionParameters.ValueType.Int) return animatorParameter?.field_Public_AvatarParameter_0?.prop_Int32_0 ?? 0;
                }
            }

            return 0;
        }
    }

    public class VRCPimaxEyeTrackerMod : MelonMod
    {
        private static Pimax.EyeTracking.EyeTracker eyeTracker;
        private static bool isChangingActiveStatus = false;
        private static bool needsExpressionUpdate = false;
        private static DateTime changeActiveStateTimer;

        private const float TIMER_CHANGE_ACTIVE_STATE = 3.0f;

        public override void OnApplicationStart()
        {
            DependencyManager.Init();

            eyeTracker = new Pimax.EyeTracking.EyeTracker();
            eyeTracker.OnUpdate += OnEyeTrackerUpdate;
            eyeTracker.OnStart += OnEyeTrackerStart;
            eyeTracker.OnStop += OnEyeTrackerStop;

            MelonCoroutines.Start(UpdateEyeTrackerState());
        }

        public override void OnApplicationQuit()
        {
            if (eyeTracker?.Active ?? false) eyeTracker.Stop();
        }

        private static void OnEyeTrackerStart()
        {
            MelonLogger.Msg("Eye Tracker Started");
            isChangingActiveStatus = false;
        }

        private static void OnEyeTrackerStop()
        {
            MelonLogger.Msg("Eye Tracker Stopped");
            isChangingActiveStatus = false;
        }

        private static void OnEyeTrackerUpdate()
        {
            needsExpressionUpdate = true;
        }

        private static void UpdateExpressionParameters()
        {
            if (eyeTracker?.Active ?? false)
            {
                ExpressionParameterManager.SetParameter("LeftEyeBlink", eyeTracker.LeftEye.Expression.Blink, true);
                ExpressionParameterManager.SetParameter("RightEyeBlink", eyeTracker.RightEye.Expression.Blink, true);

                ExpressionParameterManager.SetParameter("LeftEyeLid", eyeTracker.LeftEye.Expression.Openness, true);
                ExpressionParameterManager.SetParameter("RightEyeLid", eyeTracker.RightEye.Expression.Openness, true);

                ExpressionParameterManager.SetParameter("LeftEyeX", eyeTracker.LeftEye.Expression.PupilCenter.x);
                ExpressionParameterManager.SetParameter("RightEyeX", eyeTracker.RightEye.Expression.PupilCenter.x);
                ExpressionParameterManager.SetParameter("EyesX", eyeTracker.RightEye.Expression.Blink ? eyeTracker.LeftEye.Expression.PupilCenter.x : eyeTracker.RightEye.Expression.PupilCenter.x);

                ExpressionParameterManager.SetParameter("LeftEyeY", eyeTracker.LeftEye.Expression.PupilCenter.y);
                ExpressionParameterManager.SetParameter("RightEyeY", eyeTracker.RightEye.Expression.PupilCenter.y);
                ExpressionParameterManager.SetParameter("EyesY", eyeTracker.RightEye.Expression.Blink ? eyeTracker.LeftEye.Expression.PupilCenter.y : eyeTracker.RightEye.Expression.PupilCenter.y);
            }
        }
        
        private static IEnumerator UpdateEyeTrackerState()
        {
            while (true)
            {
                // Start or stop eye tracker from avatar quick menu
                var useEyeTracker = ExpressionParameterManager.GetBoolParameter("UseEyeTracker");
                var isEyeTrackerActive = eyeTracker?.Active ?? false;

                if (isEyeTrackerActive && needsExpressionUpdate)
                {
                    needsExpressionUpdate = false;
                    UpdateExpressionParameters();
                }

                if (!isChangingActiveStatus)
                {
                    if (isEyeTrackerActive && !useEyeTracker)
                    {
                        MelonLogger.Msg("Eye Tracker Stopping");
                        isChangingActiveStatus = true;
                        changeActiveStateTimer = DateTime.Now;
                        eyeTracker.Stop();
                    }
                    else if (!isEyeTrackerActive && useEyeTracker)
                    {
                        MelonLogger.Msg("Eye Tracker Starting");
                        isChangingActiveStatus = eyeTracker.Start();
                        changeActiveStateTimer = DateTime.Now;
                    }
                }
                else
                {
                    if ((DateTime.Now - changeActiveStateTimer).TotalSeconds >= TIMER_CHANGE_ACTIVE_STATE)
                    {
                        MelonLogger.Msg("Eye Tracker State Change Timed Out");
                        isChangingActiveStatus = false;
                        ExpressionParameterManager.SetParameter("UseEyeTracker", isEyeTrackerActive, true);
                    }
                }

                yield return new WaitForSeconds(isEyeTrackerActive ? 0.01f : 0.5f);
            }
        }
    }
}