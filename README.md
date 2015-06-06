# AcklenAvenue.Queueing

## Installation:

```
install-package AcklenAvenue.Queueing
```
and/or
```
install-package AcklenAvenue.Queueing.LocalFile
```

## Local File Queue Example Usage:

```
    public class CommandQueue : LocalFileBasedQueue<CommandQueueItem>
    {
        public CommandQueue(string queueFilePath) : base(queueFilePath)
        {
        }

        protected override List<CommandQueueItem> PrepareItemsForReturnToTextFile(List<CommandQueueItem> list)
        {
            // Transform the queue item in any way necessary.
            // Sometimes you might need to modify how the item is serialized. This is the place.
            IEnumerable<CommandQueueItem> queueItems = list
                .Select(x => new CommandQueueItem(x.UserSession, x.Command, x.Type));

            return queueItems.ToList();
        }
    }
```
