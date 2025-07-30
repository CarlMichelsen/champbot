using Database.Util;

namespace Database.Entity.Id;

public class MessageEntityId(Guid value, bool allowWrongVersion = false)
    : TypedGuid<MessageEntity>(value, allowWrongVersion);