<template id="event-details-template">
    <td class="borderless" colspan="9999">
        <div class="row justify-content-end">
            <div class="col-11">
                <div class="card shadow-nohover">
                    <div class="card-body">
                        <div class="table-responsive">
                            <table class="table table-sm borderless">
                                <thead>
                                    <tr>
                                        <td>
                                            <button class="btn btn-link" v-on:click="OrderByClicked('EventId')" v-bind:disabled="Loading">
                                                <span v-html="OrderState('EventId')"></span>#
                                            </button>
                                        </td>
                                        <td>
                                            <button class="btn btn-link" v-on:click="OrderByClicked('Name')" v-bind:disabled="Loading">
                                                <span v-html="OrderState('Name')"></span>Sự kiện
                                            </button>
                                        </td>
                                        <td>
                                            <button class="btn btn-link" v-on:click="OrderByClicked('DateOfEvent')" v-bind:disabled="Loading">
                                                <span v-html="OrderState('DateOfEvent')"></span>Ngày sự kiện
                                            </button>
                                        </td>
                                        <td>
                                            <button class="btn btn-link" v-on:click="OrderByClicked('CreateTime')" v-bind:disabled="Loading">
                                                <span v-html="OrderState('CreateTime')"></span>Ngày tạo
                                            </button>
                                        </td>
                                        <td>
                                            <button class="btn btn-link" v-on:click="OrderByClicked('Filetype')" v-bind:disabled="Loading">
                                                <span v-html="OrderState('Filetype')"></span>Loại file
                                            </button>
                                        </td>
                                        <td>
                                            <button class="btn btn-link" v-on:click="OrderByClicked('Username')" v-bind:disabled="Loading">
                                                <span v-html="OrderState('Username')"></span>Người đăng
                                            </button>
                                        </td>
                                        <td>
                                            Ghi chú
                                        </td>
                                        <td>
                                            Chỉnh sửa
                                        </td>
                                    </tr>
                                </thead>
                                <tr v-for="event in Items">
                                    <td class="top-border"><span>{{event.EventId}}</span></td>
                                    <!--<td class="top-border"><span>{{event.Name}}</span></td>-->
                                    <template v-if="IsEditMode(event.EventId)">
                                        <td class="top-border">
                                            <input type="text" class="form-control" v-model="event.Name" />
                                        </td>
                                    </template>
                                    <template v-else>
                                        <td class="top-border">
                                            <span>{{event.Name}}</span>
                                        </td>
                                    </template>
                                    <!--<td class="top-border"><span>{{event.DateOfEventString}}</span></td>-->
                                    <template v-if="IsEditMode(event.EventId)">
                                        <td class="top-border">
                                            <input type="date" class="form-control" v-model="event.DateOfEventJS" />
                                        </td>
                                    </template>
                                    <template v-else>
                                        <td class="top-border">
                                            <span>{{event.DateOfEventString}}</span>
                                        </td>
                                    </template>
                                    <td class="top-border"><span>{{event.CreateTimeString}}</span></td>
                                    <td class="top-border"><span>{{event.Filetype}}</span></td>
                                    <td class="top-border"><span>{{event.Username}}</span></td>
                                    <!--<td class="top-border"><div class="wrap-text">{{event.Note}}</div></td>-->
                                    <template v-if="IsEditMode(event.EventId)">
                                        <td class="top-border">
                                            <input type="text" class="form-control" v-model="event.Note" />
                                        </td>
                                    </template>
                                    <template v-else>
                                        <td class="top-border">
                                            <span>{{event.Note}}</span>
                                        </td>
                                    </template>
                                    <td class="top-border">
                                        <template v-if="IsEditMode(event.EventId)">
                                            <!--Clear changes button-->
                                            <button class="btn btn-outline-warning"
                                                    v-on:click="ExitEditMode(event.EventId)">
                                                <span class="fas fa-times"></span>
                                            </button>
                                        </template>
                                        <template v-else>
                                            <!--Enter edit-->
                                            <button class="btn btn-outline-primary"
                                                    v-on:click="EnterEditMode(event.EventId)">
                                                <span class="fas fa-edit"></span>
                                            </button>
                                        </template>
                                        <!--Delete-->
                                        <button v-bind:disabled="IsEditMode(event.EventId)"
                                                v-bind:class="{'btn btn-outline-secondary': IsEditMode(event.EventId), 'btn btn-outline-danger': !IsEditMode(event.EventId)}">
                                            <span class="fas fa-trash-alt"></span>
                                        </button>
                                        <!--Save changes-->
                                        <button v-bind:class="{'btn btn-outline-success': IsEditMode(event.EventId), 'btn btn-outline-secondary': !IsEditMode(event.EventId)}"
                                                v-bind:disabled="!IsEditMode(event.EventId)">
                                            <span class="fas fa-save"></span>
                                        </button>
                                    </td>
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
                                    :container-class="'pagination pagination-sm no-bottom-margin justify-content-center'">
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
    import queryBuilder from 'query-string'

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
                ItemsCopy: [],
                TotalRows: 0,
                TotalPages: 0
            };
        },
        computed: {
            //Only show page nav if total pages > 1
            CanShowPageNav: function () {
                return this.$data.TotalPages > 1;
            },
            GetCurrentItemsAPI: function () {
               return API.EventListingAPI + queryBuilder.stringify(this.ComposeCurrentItemsQuery(this.$data.OnPage));
            },
        },
        created: function () {
            //Listen to parent's event
            this.$parent.$on('populateevents', this.Populate);
        },
        methods: {
            ComposeCurrentItemsQuery: function (pageNumber) {
                return {
                    id: this.id,
                    page: pageNumber,
                    order: this.$data.OrderBy,
                    asc: this.$data.OrderAsc,
                };
            },
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
                        that.RefreshCopy(); //Clone Items to arr
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
            RefreshCopy: function () {
                var i = this.$data.Items.length;
                while (i--) this.$data.ItemsCopy[i] = JSON.parse(JSON.stringify(this.$data.Items[i]));
            },
            ExitEditMode: function (id) {
                var index = this.FindItemIndex(id);
                if (index == -1) {
                    console.log('RevertItem: Cant find ' + id);
                    return;
                }
                var clone = JSON.parse(JSON.stringify(this.$data.ItemsCopy[index]));
                console.log(clone);
                this.$data.Items[index] = clone;
                this.$forceUpdate();
            },
            //Values changed tracking
            //Edit mode
            EnterEditMode: function (id) {
                var index = this.FindItemIndex(id);
                if (index == -1) {
                    console.log('EnterEditMode: Cant find ' + id);
                    return;
                }
                //Mark item as edit
                this.$data.Items[index].EditMode = true;
                //Re-render
                this.$forceUpdate();
            },
            IsEditMode: function (id) {
                var index = this.FindItemIndex(id);
                if (index == -1) {
                    console.log('IsEditMode: Cant find ' + id);
                    return;
                }
                if (this.$data.Items[index].EditMode) {
                    return true;
                }
                return false;
            },
            FindItemIndex: function (id) {
                return this.$data.Items.findIndex(x => x.EventId == id);
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
    .no-bottom-margin{
        margin-bottom: 0;
    }
    .shadow-nohover {
        box-shadow: 0 1px 3px rgba(0,0,0,0.12), 0 1px 2px rgba(0,0,0,0.24);
    }
</style>