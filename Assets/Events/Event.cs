using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event : MonoBehaviour
{
    protected EventManager eventManager;
    protected DoorController doorController;

    



    public abstract void StartEvent();

    public void FinishEvent()
    {
        eventManager.IncrementEventCount();     //���Ӵ��������¼�����

        if (doorController != null)
        {
            doorController.OpenDoors();
        }
    }




    public void SetEventManager(EventManager manager)
    {
        eventManager = manager;
    }

    public void SetDoor(DoorController thisdoorController)
    {
        doorController = thisdoorController;
    }
}
