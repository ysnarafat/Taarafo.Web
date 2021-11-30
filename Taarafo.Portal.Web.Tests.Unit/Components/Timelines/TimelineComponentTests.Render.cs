﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Bunit;
using FluentAssertions;
using Moq;
using Taarafo.Portal.Web.Models.PostViews;
using Taarafo.Portal.Web.Models.Views.Components.Timelines;
using Taarafo.Portal.Web.Views.Bases;
using Taarafo.Portal.Web.Views.Components.Timelines;
using Xunit;

namespace Taarafo.Portal.Web.Tests.Unit.Components.Timelines
{
    public partial class TimelineComponentTests : TestContext
    {
        [Fact]
        public void ShouldInitComponent()
        {
            // given
            TimeLineComponentState expectedState =
                TimeLineComponentState.Loading;

            // when
            var initialTimelineComponent =
                new TimelineComponent();

            // then
            initialTimelineComponent.State.Should().Be(expectedState);
            initialTimelineComponent.PostViewService.Should().BeNull();
            initialTimelineComponent.PostViews.Should().BeNull();
            initialTimelineComponent.Label.Should().BeNull();
            initialTimelineComponent.ErrorMessage.Should().BeNull();
        }

        [Fact]
        public void ShouldRenderPosts()
        {
            // given
            TimeLineComponentState expectedState =
                TimeLineComponentState.Content;

            List<PostView> randomPostViews =
                CreateRandomPostViews();

            List<PostView> retrievedPostViews =
                randomPostViews;

            List<PostView> expectedPostViews =
                retrievedPostViews;

            this.postViewServiceMock.Setup(service =>
                service.RetrieveAllPostViewsAsync())
                    .ReturnsAsync(retrievedPostViews);

            // when
            this.renderedTimelineComponent =
                RenderComponent<TimelineComponent>();

            // then
            this.renderedTimelineComponent.Instance.State
                .Should().Be(expectedState);

            this.renderedTimelineComponent.Instance.PostViews
                .Should().BeEquivalentTo(expectedPostViews);

            IReadOnlyList<IRenderedComponent<CardBase>> postComponents =
                this.renderedTimelineComponent.FindComponents<CardBase>();

            postComponents.Should().HaveCount(expectedPostViews.Count);

            this.postViewServiceMock.Verify(service =>
                service.RetrieveAllPostViewsAsync(),
                    Times.Once);

            this.postViewServiceMock.VerifyNoOtherCalls();
        }
    }
}