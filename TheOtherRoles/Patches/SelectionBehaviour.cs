using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.Button;

namespace TheOtherRoles.Patches
{
    public interface IObserver<in T>
    {
        void Notify(T value);
    }

    public interface IObservable<out T>
    {
        void Subscribe(IObserver<T> observer);
    }

    public class SelectionBehaviour : IObserver<SelectionBehaviour>
    {
        public TranslationInfo title { get; private set; }
        public GameObject contents { get; private set; }
        public bool defaultValue { get; private set; }
        public Transform _transform { get; private set; }
        public Func<bool> onClick { get; private set; }

        public class InitDesc
        {
            public ToggleButtonBehaviour buttonPrefab;
            public Vector3 pos;
            public Transform parent;
            public SelectionBehaviourObservable observable;
            public Color selectColor = Color.green;
            public Color unselectColor = Palette.ImpostorRed;
            public Color mouseOverColor = new Color32(34, 139, 34, byte.MaxValue);
            public TMPro.TMP_FontAsset font = null;
            public float fontSize = 2.5f;
            public float buttonScale = 1.0f;
            public Vector2 buttonSize = new Vector2(2f, 2f);
            public Vector2 colliderButtonSize = new Vector2(2.2f, .7f);
            public string buttonName = "";
            public TMPro.TextAlignmentOptions textAlignment = TMPro.TextAlignmentOptions.Center;
            public Vector2 textOffset = Vector2.zero;
        }

        Color selectColor;
        Color unselectColor;
        Color mouseOverColor;
        SelectionBehaviourObservable observable;
        ToggleButtonBehaviour button;
        ButtonClickedEvent onButtonClick;
        string buttonName;

        public SelectionBehaviour(TranslationInfo title, Func<bool> onClick, bool defaultValue = false, GameObject contents = null) {
            this.title = title;
            this.onClick = onClick;
            this.contents = contents;
            this.defaultValue = defaultValue;
        }

        public void Initialize(InitDesc desc) {
            if (button == null)
                button = UnityEngine.Object.Instantiate(desc.buttonPrefab, desc.parent);

            _transform = button.transform;
            _transform.localPosition = desc.pos;
            _transform.localScale = Vector3.one;

            observable = desc.observable;
            selectColor = desc.selectColor;
            unselectColor = desc.unselectColor;
            mouseOverColor = desc.mouseOverColor;

            button.onState = defaultValue;
            button.Background.color = GetBackgroundColor();

            buttonName = desc.buttonName;
            UpdateText();
            button.Text.fontSizeMin = button.Text.fontSizeMax = desc.fontSize;
            button.Text.font = UnityEngine.Object.Instantiate(desc.font);
            button.Text.alignment = desc.textAlignment;
            button.Text.GetComponent<RectTransform>().sizeDelta = desc.buttonSize;
            button.Text.lineSpacing = -20f;
            button.Text.gameObject.transform.localPosition = new Vector3(button.Text.gameObject.transform.localPosition.x + desc.textOffset.x, button.Text.gameObject.transform.localPosition.y + desc.textOffset.y, button.Text.gameObject.transform.localPosition.z);
            button.name = title.GetString().Replace(" ", "") + "Toggle";
            button.gameObject.SetActive(true);

            var passiveButton = button.GetComponent<PassiveButton>();
            var colliderButton = button.GetComponent<BoxCollider2D>();

            colliderButton.size = desc.colliderButtonSize * desc.buttonScale;

            passiveButton.OnClick = new ButtonClickedEvent();
            passiveButton.OnMouseOut = new UnityEvent();
            passiveButton.OnMouseOver = new UnityEvent();

            passiveButton.OnClick.AddListener((Action)(() => {
                if (ChangeButtonState(onClick()))
                    observable?.OnChanged(this);
            }));
            onButtonClick = passiveButton.OnClick;

            passiveButton.OnMouseOver.AddListener((Action)(() => button.Background.color = mouseOverColor));
            passiveButton.OnMouseOut.AddListener((Action)(() => button.Background.color = GetBackgroundColor()));
            foreach (var spr in button.gameObject.GetComponentsInChildren<SpriteRenderer>())
                spr.size = desc.colliderButtonSize;

            observable?.Subscribe(this);
        }

        public Color GetBackgroundColor() {
            return button.onState ? selectColor : unselectColor;
        }

        public void Select() {
            onButtonClick?.Invoke();
        }

        public void Notify(SelectionBehaviour value) {
            if (this == value)
                return;
            if (value.button.onState)
                ChangeButtonState(false);
        }

        public bool ChangeButtonState(bool onState) {
            button.onState = onState;
            UpdateUI();
            return true;
        }

        public void SetActive(bool isActive) {
            if (_transform == null) return;
            if (_transform.gameObject == null) return;
            _transform.gameObject.SetActive(isActive);
        }

        public void UpdateText() {
            button.Text.text = string.IsNullOrEmpty(buttonName) ? title.GetString() : buttonName;
        }

        void UpdateUI() {
            button.Background.color = GetBackgroundColor();
            if (contents != null)
                contents.SetActive(button.onState);
        }
    }


    public class SelectionBehaviourObservable : IObservable<SelectionBehaviour>
    {
        private List<IObserver<SelectionBehaviour>> observerList = new List<IObserver<SelectionBehaviour>>();

        public void Subscribe(IObserver<SelectionBehaviour> observer) {
            if (!observerList.Contains(observer))
                observerList.Add(observer);
        }

        public void OnChanged(SelectionBehaviour value) {
            foreach (var observer in observerList) {
                observer.Notify(value);
            }
        }

        public void Clear() {
            observerList.Clear();
        }
    }
}
