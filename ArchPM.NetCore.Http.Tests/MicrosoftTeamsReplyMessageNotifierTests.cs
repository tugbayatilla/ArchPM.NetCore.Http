//using System;
//using System.Threading.Tasks;
//using ArchPM.NetCore.Builders;
//using ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Clients;
//using ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Messages;
//using ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Notifiers;
//using FluentAssertions;
//using Moq;
//using Xunit;

//namespace ArchPM.NetCore.Http.Tests
//{
//    public class MicrosoftTeamsReplyMessageNotifierTests
//    {
//        private readonly Mock<IMicrosoftTeamsLogicAppReplyMessageClient> _clientMock = new Mock<IMicrosoftTeamsLogicAppReplyMessageClient>();
        
//        private MicrosoftTeamsReplyMessageNotifier CreateNotifier()
//        {
//            var notifier = new MicrosoftTeamsReplyMessageNotifier(_clientMock.Object);

//            return notifier;
//        }

//        [Fact]
//        public async Task NotifyAsync_Should_throw_argument_null_exception_when_argument_is_null()
//        {

//            var notifier = CreateNotifier();

//            await Assert.ThrowsAsync<ArgumentNullException>(
//                () => notifier.NotifyAsync(null)
//            );
//        }

//        [Fact]
//        public async Task NotifyAsync_Should_throw_argument_exception_when_message_is_null()
//        {
//            var notifier = CreateNotifier();
//            var message = SampleBuilder.Create<DefaultMicrosoftTeamsReplyMessage>(
//                p => { p.Message = null; });

//            await Assert.ThrowsAsync<ArgumentException>(
//                () => notifier.NotifyAsync(message)
//            );
//        }

//        [Fact]
//        public async Task NotifyAsync_Should_throw_argument_exception_when_messageId_is_null()
//        {
//            var notifier = CreateNotifier();
//            var message = SampleBuilder.Create<DefaultMicrosoftTeamsReplyMessage>(
//                p => { p.MessageId = null; });

//            await Assert.ThrowsAsync<ArgumentException>(
//                () => notifier.NotifyAsync(message)
//            );
//        }

//        [Fact]
//        public async Task NotifyAsync_Should_throw_exception_when_response_message_is_null()
//        {
//            _clientMock.Setup(m => m.SendMessage(It.IsAny<IMicrosoftTeamsReplyMessage>()))
//                .ReturnsAsync(() => null);

//            var notifier = CreateNotifier();
//            var message = SampleBuilder.Create<DefaultMicrosoftTeamsReplyMessage>();

//            await Assert.ThrowsAsync<Exception>(
//                () => notifier.NotifyAsync(message)
//            );
//        }

//        [Fact]
//        public async Task NotifyAsync_Should_throw_exception_when_response_messageId_is_null()
//        {
//            _clientMock.Setup(m => m.SendMessage(It.IsAny<IMicrosoftTeamsReplyMessage>()))
//                .ReturnsAsync(() => new DefaultMicrosoftTeamsReplyMessageResponse()
//                {
//                    MessageId = null
//                });

//            var notifier = CreateNotifier();
//            var message = SampleBuilder.Create<DefaultMicrosoftTeamsReplyMessage>();

//            await Assert.ThrowsAsync<Exception>(
//                () => notifier.NotifyAsync(message)
//            );
//        }

//        [Fact]
//        public async Task NotifyAsync_Should_return_valid_response()
//        {
//            _clientMock.Setup(m => m.SendMessage(It.IsAny<IMicrosoftTeamsReplyMessage>()))
//                .ReturnsAsync(
//                    () => SampleBuilder.Create<DefaultMicrosoftTeamsReplyMessageResponse>()
//                );

//            var notifier = CreateNotifier();
//            var message = SampleBuilder.Create<DefaultMicrosoftTeamsReplyMessage>();

//            var response = await notifier.NotifyAsync(message);
//            response.Should().NotBeNull();
//        }
//    }
//}
