using System;
using UnityEngine;
using UnityEngine.Serialization;
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
        
        
        [HideInInspector]
        public Team team = Team.None;
        [HideInInspector]
        public TileState tileState = TileState.Base;

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

            if (team == Team.None)
            {
                if (playerClick)
                {
                    image.sprite = _playerBaseSprite;
                    team = Team.Human;
                }
                else
                {
                    image.sprite = _compBaseSprite;
                    team = Team.Cpu;
                }
            }
            else
            {
                if (playerClick)
                {
                    image.sprite = _playerUpgSprite;
                    tileState = TileState.Upgraded;
                }
                else
                {
                    image.sprite = _compUpgSprite;
                    tileState = TileState.Upgraded;
                }
            }
            
        }
        
    }
}