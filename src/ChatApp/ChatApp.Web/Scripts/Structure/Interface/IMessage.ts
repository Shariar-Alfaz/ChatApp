interface IMessage {
    id: string;
    senderId: string;
    receiverId: string;
    text: string;
    sendingTime: Date;
}