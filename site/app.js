angular.module('BlogApp', ['ngRoute'])
    .config(function ($routeProvider) {
        $routeProvider.
            when('/', {
                templateUrl: 'post_list.html',
                controller: 'BlogPostListController as blogPostListController'
            }).
            when('/post/:postId', {
                templateUrl: 'post.html',
                controller: 'BlogPostController as blogPostController'
            })
            .otherwise({
                redirectTo: '/'
            });
    }
    )
.controller('BlogPostListController', function ($http, $scope) {
    var blogPostList = this;
    $http.get('/get_latest_blog_posts').success(function (data) {
        blogPostList.posts = data;
    })
})
.controller('BlogPostController', function ($http, $scope, $routeParams) {
    var blogPost = this;
    $http.get('/get_blog_post/' + $routeParams.postId).success(function (data) {
        blogPost.post = data;
    })
})
