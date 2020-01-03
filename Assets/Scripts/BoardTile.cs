using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    
    public class BoardTile : MonoBehaviour
    {
        private static Sprite _defaultSprite;
        private static Sprite _playerBaseSprite;
        private static Sprite _playerUpgSprite;
        private static Sprite _compBaseSprite;
        private static Sprite _compUpgSprite;
        
        private enum Team
        {
            Human,
            Cpu,
            None
        }

        private enum State
        {
            Base,
            Upgraded
        }

        private Team _team = Team.None;
        private State _state = State.Base;

        [HideInInspector] public int row = 0;
        [HideInInspector] public int column = 0;

        private void Start() {
            _defaultSprite = Resources.Load<Sprite>("Sprites/EmptyTile");
            _playerBaseSprite = Resources.Load<Sprite>("Sprites/PlayerBaseTile");
            _playerUpgSprite = Resources.Load<Sprite>("Sprites/PlayerUpgTile");
            _compBaseSprite = Resources.Load<Sprite>("Sprites/ComputerBaseTile");
            _compUpgSprite = Resources.Load<Sprite>("Sprites/ComputerUpgTile");
        }

        public void MarkTile(bool playerClick)
        {
            var image = GetComponent<Image>();
            if (playerClick)
            {
                image.sprite = _playerBaseSprite;
                _team = Team.Human;
            }
            else
            {
                image.sprite = _compBaseSprite;
                _team = Team.Cpu;
            }

            var btn = GetComponent<Button>();
            btn.enabled = false;
        }
        
    }
}