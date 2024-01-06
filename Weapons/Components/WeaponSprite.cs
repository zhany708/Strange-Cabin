using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZhangYu.Weapons
{
    public class WeaponSprite : WeaponComponent
    {
        SpriteRenderer m_BaseSpriteRenderer;
        SpriteRenderer m_WeaponSpritRenderer;

        [SerializeField] private WeaponSprites[] m_WeaponSprites;    //�������й�������


        int m_CurrentWeaponSpriteIndex;

        protected override void Awake()
        {
            base.Awake();

            m_BaseSpriteRenderer = transform.Find("Base").GetComponent<SpriteRenderer>();
            m_WeaponSpritRenderer = transform.Find("WeaponSprite").GetComponent<SpriteRenderer>();

            //m_BaseSpriteRenderer = weapon.BaseGameObject.GetComponent<SpriteRenderer>();
            //m_WeaponSpritRenderer = weapon.WeaponSpriteGameObject.GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            m_BaseSpriteRenderer.RegisterSpriteChangeCallback(HandleBaseSpriteChange);

            weapon.OnEnter += HandleEnter;
        }

        private void OnDisable()
        {
            m_BaseSpriteRenderer.UnregisterSpriteChangeCallback(HandleBaseSpriteChange);

            weapon.OnEnter -= HandleEnter;
        }



        private void HandleBaseSpriteChange(SpriteRenderer spriteRenderer)
        {
            m_WeaponSpritRenderer.sprite = m_WeaponSprites[weapon.CurrentAttackCounter].Sprites[m_CurrentWeaponSpriteIndex];    //���ݵ�ǰ������ѡ����ȷ�ľ���ͼ

            m_CurrentWeaponSpriteIndex++;
        }

        private void HandleEnter() => m_CurrentWeaponSpriteIndex = 0;   //ÿ����ʼ����ʱ������������ͼ���������
    }


    [Serializable]
    public class WeaponSprites
    {
        [field: SerializeField] public Sprite[] Sprites { get; private set; }
    }
}
