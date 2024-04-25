using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Commandoo {

    public class CMDEditorCoroutine {

        private readonly IEnumerator _routine;

        public static CMDEditorCoroutine StartEditorCoroutine(IEnumerator routine) {
            CMDEditorCoroutine coroutine = new CMDEditorCoroutine(routine);
            coroutine.Start();
            return coroutine;
        }

        private CMDEditorCoroutine(IEnumerator routine) {
            _routine = routine;
        }

        private void Start() {
            EditorApplication.update += Update;
        }

        private void Update() {
            if (!_routine.MoveNext()) {
                StopEditorCoroutine();
            }
        }

        public void StopEditorCoroutine() {
            EditorApplication.update -= Update;
        }
    }
}