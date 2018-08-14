using UnityEngine;
using System.Collections;

public interface IAcceptsMessages<T> where T : BaseMessage
{
    void AcceptMessage(T messageIn);
}
