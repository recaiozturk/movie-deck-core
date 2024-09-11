$(document).ready(function () {
    $('#MovieListType').select2({
        placeholder: "Choose List Type",
        allowClear: true
    });

    $('body').on('click', '.opening_modal_delete', function () {
        let id = this.id;
        $("#listDeleteAccept").click(function () {
            DeleteList(id)
        });
    });

    $('#inputSearch').on('input', function () {
        $('.loading-spinner').addClass('d-block');
        $('.loading-spinner').removeClass('d-none');
        var searchText = $(this).val();
        if (searchText.length > 0) {

            $.ajax({
                url: '/MovieList/MovieSearch',
                method: 'GET',
                data: { value: searchText },
                success: function (response) {

                    var movies = response.data;
                    $('#searchResult').empty();

                    if (response.isValid && response.data.length > 0) {
                        $('.loading-spinner').addClass('d-none');
                        $('.loading-spinner').removeClass('d-block');

                        $.each(movies, function (index, movie) {
                            $('#searchResult').append(`<div id="${movie.id}"  class="movie"> ${movie.title} (${movie.release_date})</div>`);
                        });
                    } else {

                        $('.loading-spinner').addClass('d-none');
                        $('.loading-spinner').removeClass('d-block');
                        $('#searchResult').append('<div class="movie">No results found</div>');
                    }
                },
                error: function (xhr, status, error) {
                }
            });
        } else {
            $('.loading-spinner').addClass('d-none');
            $('.loading-spinner').removeClass('d-block');
            $('#searchResult').empty();
        }
    });

    //listeye film ekleme
    $('.search-results').on('click', '.movie', function () {

        $('.t-center').addClass('d-block');
        $('.t-center').removeClass('d-none');

        var _movieId = $(this).attr('id');
        var _listId = $("#movieListID").val();

        $.ajax({
            url: '/MovieList/AddMovieToList',
            method: 'POST',
            data: { movieId: _movieId, listId: _listId },
            success: function (response) {
                $('.search-results').empty();

                if (response.isValid) {

                    var gonnaAppendMovie =
                        `
                        <div id="movieHis_${response.data.movieHistoryId}"  class="col-12">
                            <div class="profile__content px-2">
                                <!-- profile user -->
                                <div class="profile__user">

                                    <div class="section__title mr-2"><p class="p-2">${response.data.displayOrder}</p></div>

                                        <div class="profile__avatar">
                                            <img src="https://media.themoviedb.org/t/p/w220_and_h330_face/${response.data.poster_path}" alt="">
                                        </div>
                                        <!-- or red -->
                                        <div class="profile__meta profile__meta--green">
                                            <h3>${response.data.title}</h3>
                                            <span>${response.data.release_date}</span>
                                        </div>
                                    </div>

                                    <!-- profile btns -->
                                    <div class="profile__actions">
                                        <button onclick="UpOrder(${response.data.movieHistoryId},${LISTID})" class="profile__action profile__action--banned "><i class="icon ion-ios-arrow-up"></i></button>

                                        <button onclick="DownOrder(${response.data.movieHistoryId},${LISTID})" class="profile__action profile__action--banned "><i class="icon ion-ios-arrow-down"></i></button>


                                            <button onclick="DeleteMovieFromList(${response.data.movieHistoryId})" class="profile__action profile__action--delete"><div id="btnSpinner_${response.data.movieHistoryId}"><i class="icon ion-ios-trash"></i></div></button>
                                                </div>
                                                <!-- end profile btns -->
                                            </div>
                            </div>`;

                    $('#listMovies').append(gonnaAppendMovie);


                    $('.t-center').addClass('d-none');
                    $('.t-center').removeClass('d-block');

                    MyToast.fire({
                        icon: 'success',
                        title: response.successMessage
                    })


                } else {
                    $('.t-center').addClass('d-none');
                    $('.t-center').removeClass('d-block');
                    alert("error")
                    //toast error
                }
            },
            error: function (xhr, status, error) {
            }
        });

    });

});

function DeleteList(listId) {
    event.preventDefault();
    $.ajax({
        url: '/MovieList/DeleteList',
        method: 'POST',
        data: { listId: listId },
        success: function (response) {

            if (response.isValid) {

                MyToast.fire({
                    icon: 'success',
                    title: response.message
                }).then(function () {
                    location.href = '/Profile';
                })

            } else {
                MyToast.fire({
                    icon: 'error',
                    title: response.message
                })
                //toast error
            }
        },
        error: function (xhr, status, error) {
        }
    });
}

function DeleteMovieFromList(movieHistId, listId) {
    event.preventDefault();
    $("#btnSpinner_" + movieHistId).empty().append('<img src="https://i.gifer.com/ZKZg.gif" alt="Loading..." style="width: 100%;">');

    $.ajax({
        url: '/MovieList/RemoveMovieFromList',
        method: 'POST',
        data: { movieListHistoryId: movieHistId, movieListId: listId },
        success: function (response) {
            console.log(response)
            if (response.isValid) {

                var gonnaDeletedMovie = $("#movieHis_" + movieHistId);
                gonnaDeletedMovie.remove();
                -

                    $('.t-center').addClass('d-none');
                $('.t-center').removeClass('d-block');

                MyToast.fire({
                    icon: 'success',
                    title: response.successMessage
                }).then(function () {
                    location.reload();
                })

            } else {
                $('.t-center').addClass('d-none');
                $('.t-center').removeClass('d-block');
                $("#btnSpinner_" + movieHistId).empty().append('<i class="icon ion-ios-trash"></i>');
                alert("error")
                //toast error
            }
        },
        error: function (xhr, status, error) {
        }
    });
}

function UpOrder(movieHistId, listId) {
    event.preventDefault();

    $.ajax({
        url: '/MovieList/UpOrder',
        method: 'POST',
        data: { movieListHistoryId: movieHistId, listId: listId },
        success: function (response) {
            console.log(response)

            if (response.isValid) {

                MyToast.fire({
                    icon: 'success',
                    title: response.successMessage
                }).then(function () {
                    location.reload();
                })

            } else {
                MyToast.fire({
                    icon: 'error',
                    title: response.errorMessage
                })
            }
        },
        error: function (xhr, status, error) {
        }
    });
}

function DownOrder(movieHistId, listId) {
    event.preventDefault();
    $.ajax({
        url: '/MovieList/DownOrder',
        method: 'POST',
        data: { movieListHistoryId: movieHistId, listId: listId },
        success: function (response) {

            if (response.isValid) {

                MyToast.fire({
                    icon: 'success',
                    title: response.successMessage
                }).then(function () {
                    location.reload();
                })
            } else {
                MyToast.fire({
                    icon: 'error',
                    title: response.errorMessage
                })
            }
        },
        error: function (xhr, status, error) {
        }
    });
}