//using System;
//using System.Threading.Tasks;
//using ArchPM.NetCore.Builders;
//using ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Clients;
//using ArchPM.NetCore.Http.Notifications.MicrosoftTeams.Messages;
//using FluentAssertions;
//using Moq;
//using Xunit;

//namespace ArchPM.NetCore.Http.Tests
//{
//    public class MicrosoftTeamsPostMessageNotifierTests
//    {
//        private readonly Mock<IMicrosoftTeamsLogicAppPostMessageClient> _clientMock = new Mock<IMicrosoftTeamsLogicAppPostMessageClient>();
        
//        private MicrosoftTeamsPostMessageNotifier CreateNotifier()
//        {
//            var notifier = new MicrosoftTeamsPostMessageNotifier(_clientMock.Object);

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
//            var message = SampleBuilder.Create<DefaultMicrosoftTeamsPostMessage>(
//                p => { p.Message = null; });

//            await Assert.ThrowsAsync<ArgumentException>(
//                () => notifier.NotifyAsync(message)
//            );
//        }

//        [Fact]
//        public async Task NotifyAsync_Should_throw_argument_exception_when_subject_is_null()
//        {
//            var notifier = CreateNotifier();
//            var message = SampleBuilder.Create<DefaultMicrosoftTeamsPostMessage>(
//                p => { p.Subject = null; });

//            await Assert.ThrowsAsync<ArgumentException>(
//                () => notifier.NotifyAsync(message)
//            );
//        }

//        [Fact]
//        public async Task NotifyAsync_Should_throw_exception_when_response_message_is_null()
//        {
//            _clientMock.Setup(m => m.SendMessage(It.IsAny<IMicrosoftTeamsPostMessage>()))
//                .ReturnsAsync(() => null);

//            var notifier = CreateNotifier();
//            var message = SampleBuilder.Create<DefaultMicrosoftTeamsPostMessage>();

//            await Assert.ThrowsAsync<Exception>(
//                () => notifier.NotifyAsync(message)
//            );
//        }

//        [Fact]
//        public async Task NotifyAsync_Should_throw_exception_when_response_messageId_is_null()
//        {
//            _clientMock.Setup(m => m.SendMessage(It.IsAny<IMicrosoftTeamsPostMessage>()))
//                .ReturnsAsync(() => new DefaultMicrosoftTeamsPostMessageResponse()
//                {
//                    MessageId = null
//                });

//            var notifier = CreateNotifier();
//            var message = SampleBuilder.Create<DefaultMicrosoftTeamsPostMessage>();

//            await Assert.ThrowsAsync<Exception>(
//                () => notifier.NotifyAsync(message)
//            );
//        }

//        [Fact]
//        public async Task NotifyAsync_Should_return_valid_response()
//        {
//            _clientMock.Setup(m => m.SendMessage(It.IsAny<IMicrosoftTeamsPostMessage>()))
//                .ReturnsAsync(
//                    () => SampleBuilder.Create<DefaultMicrosoftTeamsPostMessageResponse>()
//                );

//            var notifier = CreateNotifier();
//            var message = SampleBuilder.Create<DefaultMicrosoftTeamsPostMessage>();

//            var response = await notifier.NotifyAsync(message);
//            response.Should().NotBeNull();
//        }
//    }
//}
