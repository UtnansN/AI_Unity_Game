using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{

    public class BoardTile : MonoBehaviour
    {
        private static Sprite _defaultSprite;
        private static Sprite _playerBaseSprite;
        private static Sprite _compBaseSprite;

        [HideInInspector]
        public Team team = Team.None;

        [HideInInspector] public int row;
        [HideInInspector] public int column;

        private void Start() {
            _defaultSprite = Resources.Load<Sprite>("Sprites/EmptyTile");
            _playerBaseSprite = Resources.Load<Sprite>("Sprites/PlayerBaseTile");
            _compBaseSprite = Resources.Load<Sprite>("Sprites/ComputerBaseTile");
        }

        public void MarkTile(bool playerClick)
        {
            var image = GetComponent<Image>();

            if (team != Team.None) return;
            
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

        public void RefreshSprite()
        {
            var image = GetComponent<Image>();
            Sprite settableSprite;

            switch (team)
            {
                case Team.None:
                    settableSprite = _defaultSprite;
                    break;
                case Team.Human:
                    settableSprite = _playerBaseSprite;
                    break;
                case Team.Cpu:
                    settableSprite = _compBaseSprite;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            } 
            
            if (image.sprite != settableSprite)
            {
                image.sprite = settableSprite;
            }
        }
        
    }
}