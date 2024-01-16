using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCSetting
{
    Goppi = 0,
    Jaeppi,
    Animal,
    Shop
}

public enum ShopInfo
{
    Riding = 0,
    Clothes,
    Hair,
    Acc
}

[CreateAssetMenu(fileName = "NPCInfo_Data", menuName = "NPCInfo_Data")]
public class NPCInfo_Data : ScriptableObject
{
    [Header("NPC Info Setting")]
    public string npcName;
    public string npcText;
    public string prompt; // gpt prompt
    public NPCSetting npcSetting;

    // button setting
    [Header("TalkPanel Setting")]
    public bool micButton; // audio button
    public string language; // clova lang
    public bool npcYesButton; // talk panel�� button 1 or 2, true = 2
    public string yesButtonText;
    public string noButtonText;

    [Header("Animal Info")] // 24.01.10 �����̿� animal info ��� npc info�� animal ������ �����صα�� ���ǿ�.
    public string animal_id;

    [Header("Shop Info")]
    public ShopInfo shopInfo;
}
