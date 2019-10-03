using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IQueuesAndProcessesMessages<T> : IAcceptsMessages<T> where T : BaseMessage
{
    void queueMessage(T messageIn);

    void processMessages(ICollection<T> messages);
}

