<template id="event-details-template">
    <td class="borderless" colspan="9999">
        <div class="row justify-content-end">
            <div class="col-11">
                <div class="card shadow-nohover">
                    <div class="card-body">
                        <div class="table-responsive">
                            <table class="table table-sm borderless">
                                <thead>
                                    <tr class="text-danger">
                                        <th>
                                            <button class="btn btn-link" v-on:click="OrderByClicked('EventId')" v-bind:disabled="Loading">
                                                <span v-html="OrderState('EventId')"></span>#
                                            </button>
                                        </th>
                                        <th>
                                            <button class="btn btn-link" v-on:click="OrderByClicked('Name')" v-bind:disabled="Loading">
                                                <span v-html="OrderState('Name')"></span>Sự kiện
                                            </button>
                                        </th>
                                        <th>
                                            <button class="btn btn-link" v-on:click="OrderByClicked('DateOfEvent')" v-bind:disabled="Loading">
                                                <span v-html="OrderState('DateOfEvent')"></span>Ngày sự kiện
                                            </button>
                                        </th>
                                        <th>
                                            <button class="btn btn-link" v-on:click="OrderByClicked('CreateTime')" v-bind:disabled="Loading">
                                                <span v-html="OrderState('CreateTime')"></span>Ngày tạo
                                            </button>
                                        </th>
                                        <th>
                                            <button class="btn btn-link" v-on:click="OrderByClicked('Filetype')" v-bind:disabled="Loading">
                                                <span v-html="OrderState('Filetype')"></span>Loại file
                                            </button>
                                        </th>
                                        <th>
                                            <button class="btn btn-link" v-on:click="OrderByClicked('Username')" v-bind:disabled="Loading">
                                                <span v-html="OrderState('Username')"></span>Người đăng
                                            </button>
                                        </th>
                                        <th>
                                            Ghi chú
                                        </th>
                                        <!--<td>
                                            <button class="btn btn-sm">
                                                <span class="fa fa-plus"></span>
                                            </button>
                                        </td>-->
                                    </tr>
                                </thead>
                                <tr v-for="event in Items">
                                    <td class="top-border"><span>{{event.EventId}}</span></td>
                                    <td class="top-border"><span>{{event.Name}}</span></td>
                                    <td class="top-border"><span>{{event.DateOfEvent}}</span></td>
                                    <td class="top-border"><span>{{event.CreateTime}}</span></td>
                                    <td class="top-border"><span>{{event.Filetype}}</span></td>
                                    <td class="top-border"><span>{{event.Username}}</span></td>
                                    <td class="top-border"><div class="wrap-text">{{event.Note}}</div></td>
                                    <!--<td><button class="btn-link"><span class="fa fa-pencil-square-o"></span></button></td>-->
                                </tr>
                            </table>
                        </div>
                        <page-nav v-show="CanShowPageNav"
                                    :page-count="TotalPages"
                                    :click-handler="PageNavClicked"
                                    :page-range="2"
                                    :prev-text="'Trước'"
                                    :force-page="OnPage - 1"
                                    :next-text="'Sau'"
                                    :page-class="'page-item'"
                                    :page-link-class="'page-link'"
                                    :prev-class="'page-item'"
                                    :prev-link-class="'page-link'"
                                    :next-class="'page-item'"
                                    :next-link-class="'page-link'"
                                    :container-class="'pagination pagination-sm justify-content-center'">
                        </page-nav>
                    </div>
                </div>
            </div>
        </div>
    </td>
</template>
<script>
    import API from '../Home/API'
    import axios from 'axios'
    import pagenav from 'vuejs-paginate'

    export default {
        name: 'event-details',
        template: '#event-details-template',
        components: {
            'page-nav': pagenav
        },
        props: ['id'],
        data: function () {
            return {
                Loading: false,
                Populated: false,

                OnPage: 1,
                OrderBy: 'EventId',
                OrderAsc: true,

                Items: [],
                TotalRows: 0,
                TotalPages: 0,
            };
        },
        computed: {
            //Only show page nav if total pages > 1
            CanShowPageNav: function () {
                return this.$data.TotalPages > 1;
            },
            GetCurrentItemsAPI: function () {
                var api = API.EventListingURL;
                var page = this.$data.OnPage;
                if (page < 1 || page == null) page = 1;
                api = api.replace("{id}", this.id);
                api = api.replace("{page}", page);
                api = api.replace("{order}", this.$data.OrderBy);
                api = api.replace("{asc}", this.$data.OrderAsc);
                return api;
            },
        },
        created: function () {
            //Listen to parent's event
            this.$parent.$on('populateevents', this.Populate);
        },
        methods: {
            Populate: function (id) {
                //Check if this component being clicked
                if (id != this.id) return;
                //Check if has items
                if (this.$data.Populated) {
                    //Do nothing
                    this.$emit('populatecompleted');
                    return;
                }
                //Load items
                this.LoadItems(this.GetCurrentItemsAPI);
            },
            LoadItems: function (url) {
                //console.log(url);
                this.$data.Loading = true;
                var that = this;
                axios.get(url)
                    .then(function (response) {
                        if (response.headers.login) {
                            window.location.href = response.headers.login;
                        }
                        that.$data.Items = response.data.Items;
                        that.UpdatePagination(response.data.TotalPages, response.data.TotalRows);
                        that.$data.Loading = false;
                        that.$data.Populated = true;
                        that.$emit('populatecompleted');
                    })
                    .catch(function (error) {
                        console.log(error);
                        //Pop up error
                    });
            },
            //order methods
            OrderByClicked: function (orderBy) {
                if (this.$data.Loading) return;
                this.$data.Loading = true; //prevent click spamming
                //Flip order by
                if (this.$data.OrderBy == orderBy) {
                    this.$data.OrderAsc = !this.$data.OrderAsc;
                }
                else {
                    //Order this column
                    this.$data.OrderBy = orderBy;
                    this.$data.OrderAsc = true;
                }
                this.LoadItems(this.GetCurrentItemsAPI);
            },
            OrderState: function (orderBy) {
                //console.log(orderBy);
                if (orderBy == this.$data.OrderBy) {
                    if (this.$data.OrderAsc)
                        return '&utrif;';
                    return '&dtrif;';
                }
                return '';
            },
            PageNavClicked: function (page) {
                if (this.$data.Loading) return;
                this.$data.Loading = true;
                this.$data.OnPage = page;
                this.LoadItems(this.GetCurrentItemsAPI);
            },
            UpdatePagination: function (totalPages, totalRows) {
                this.$data.TotalPages = totalPages;
                this.$data.TotalRows = totalRows;
            },
        }
    }
</script>
<style scoped>
    table.borderless td {
        border: none;
    }
    .wrap-text{
        word-wrap: break-word;
    }
    .top-border {
        border-top: 1px solid #dee2e6 !important;
    }
    .shadow-nohover {
        box-shadow: 0 1px 3px rgba(0,0,0,0.12), 0 1px 2px rgba(0,0,0,0.24);
    }
</style>