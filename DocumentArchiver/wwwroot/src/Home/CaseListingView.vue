<template id="listing-template">
    <div>
        <div class="row top-margin">
            <div class="col-sm-8 mx-auto">
                <search-bar v-bind:isdisabled="Loading"
                            v-on:submit="SearchButtonClicked"></search-bar>
            </div>
        </div>
        <div class="row top-margin">
            <div class="col-12">
                <div class="table-responsive">
                    <table class="table table-hover">
                        <thead>
                            <tr>
                                <!--<td><button class="btn btn-link btn-group-sm" v-on:click="CollapseAll"><span class="glyphicon glyphicon-minus"></span> All</button></td>-->
                                <!--<td><button class="btn btn-link" v-on:click="OrderByClicked('ContractId')" v-bind:disabled="Loading">
                                    <span v-html="DisplayOrderButtonStates('ContractId')"></span>#</button></td>-->
                                <th><button class="btn btn-link" v-on:click="OrderByClicked('ContractNumber')" v-bind:disabled="Loading">
                                    <span v-html="OrderState('ContractNumber')"></span>Số Hd.</button></th>
                                <th><button class="btn btn-link" v-on:click="OrderByClicked('CustomerName')" v-bind:disabled="Loading">
                                    <span v-html="OrderState('CustomerName')"></span>Tên Kh.</button></th>
                                <th><button class="btn btn-link" v-on:click="OrderByClicked('IdentityCard')" v-bind:disabled="Loading">
                                    <span v-html="OrderState('IdentityCard')"></span>CMND</button></th>
                                <th><button class="btn btn-link" v-on:click="OrderByClicked('Phone')" v-bind:disabled="Loading">
                                    <span v-html="OrderState('Phone')"></span>SĐT</button></th>
                                <th><button class="btn btn-link" v-on:click="OrderByClicked('CreateTime')" v-bind:disabled="Loading">
                                    <span v-html="OrderState('CreateTime')"></span>Ngày tạo</button></th>
                                <th><button class="btn btn-link" v-on:click="OrderByClicked('Username')" v-bind:disabled="Loading">
                                    <span v-html="OrderState('Username')"></span>Người tạo</button></th>
                            </tr>
                        </thead>
                        <tbody>
                            <template v-for="item in Items">
                                <tr v-on:click="ToggleEventDetails(item.ContractId)">
                                    <!--<td>
                                    <button v-on:click="TogglePosDetails(dealer.DealerId)" class="btn-xs btn-link">
                                        <span class="glyphicon" v-bind:class="{'glyphicon-minus': IsShowingPosDetails(dealer.DealerId), 'glyphicon-plus': !IsShowingPosDetails(dealer.DealerId)}">
                                        </span>
                                    </button>
                                </td>-->
                                    <!--<td>
                                        <span class="table-cell-content">{{item.ContractId}}</span>
                                    </td>-->
                                    <td>
                                        <span class="table-cell-content">{{item.ContractNumber}}</span>
                                    </td>
                                    <td>
                                        <span class="table-cell-content">{{item.CustomerName}}</span>
                                    </td>
                                    <td>
                                        <span class="table-cell-content">{{item.IdentityCard}}</span>
                                    </td>
                                    <td>
                                        <span class="table-cell-content">{{item.Phone}}</span>
                                    </td>
                                    <td>
                                        <span class="table-cell-content">{{item.CreateTime}}</span>
                                    </td>
                                    <td>
                                        <span class="table-cell-content">{{item.Username}}</span>
                                    </td>
                                </tr>
                                <!--@*expandable details*@-->
                                <tr>
                                    <event-details 
                                                   v-bind:id="item.ContractId" 
                                                   v-show="IsShowingEventDetails(item.ContractId)"
                                                   v-on:populatecompleted="ChildLoadCompleted()"></event-details>
                                    <!--<other-details v-bind:dealer="dealer" v-show="IsShowingOtherDetails(dealer.DealerId)"></other-details>

                                    <doc-details v-on:displayscannclicked="OpenNewScanPage"
                                                 v-on:showuploadmodalclicked="ShowUploadModalHandler"
                                                 v-bind:documentnames="DocumentNames"
                                                 v-bind:uploaded="dealer.Scan"
                                                 v-bind:dealerid="dealer.DealerId"
                                                 v-show="IsShowingDocDetails(dealer.DealerId)"
                                                 v-bind:readonly=readonly
                                                 v-on:showprintdocclicked="PrinDocumentModalHandler"></doc-details>
                                    @*any better ways to do this?*@-->
                                </tr>
                            </template>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <div class="row top-margin">
            <div class="col-lg-6 mx-auto">
                <page-nav :page-count="TotalPages"
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
                          :container-class="'pagination justify-content-center'">
                </page-nav>
            </div>
        </div>
    </div>
</template>
<script>
    import axios from 'axios'    
    import common from '../Common'
    import API from '../Home/API'
    import searchBar from '../Home/SearchBar.vue'
    import eventDetails from '../Home/EventDetails.vue'
    import pagenav from 'vuejs-paginate'

    //import InputModal from './InputModal.vue'
    //import DownPopup from './DownPopup.vue'

    export default {
        name: 'CaseListingView',
        template: '#listing-template',
        components: {
            'page-nav': pagenav,
            'search-bar': searchBar,
            'event-details': eventDetails
        },
        mounted: function () {
            this.Init();
        },
        watch: {
            '$route'(to, from) {
                this.$data.OnPage = to.query.page;
                this.$data.FilterBy = to.query.type;
                this.$data.FilterString = to.query.contain
                this.$data.OrderBy = to.query.order;
                this.$data.OrderAsc = to.query.asc
                this.LoadItems(this.GetCurrentItemsAPI);
            }
        },
        data: function () {
            return {
                Loading: false,

                OnPage: 1,
                FilterBy: '',
                FilterString: '',
                OrderBy: '',
                OrderAsc: true,
                Loading: false,

                Injected: null,
                Items: [],
                TotalRows: 0,
                TotalPages: 0,

                //Details
                ShowingEventDetails: [],
            };
        },
        computed: {
            //page = { page } & type={ type } & contain={contain }&order={ order }&asc={ asc }
            GetCurrentItemsAPI: function () {
                var api = API.ContractListingURL;
                var page = this.$data.OnPage;
                if (page < 1 || page == null) page = 1;
                api = api.replace("{page}", page);
                api = api.replace("{type}", this.$data.FilterBy);
                api = api.replace("{contain}", this.$data.FilterString);
                api = api.replace("{order}", this.$data.OrderBy);
                api = api.replace("{asc}", this.$data.OrderAsc);
                return api;
            },
            //CanExport: function () {
            //    return common.arrayContains(this.$data.Ability, "ExportRequests");
            //},
            //CanSeeAll: function () {
            //    return common.arrayContains(this.$data.Ability, "SeeAllRequests");
            //}
        },
        methods: {
            //init app
            Init: function () {
                //Load injected
                var model = window.model;
                if (!model) {
                   //Show app init failed
                    return;
                }
                this.$data.Injected = model;
                this.$data.FilterBy = model.FilterBy;
                this.$data.FilterString = model.FilterString;
                this.$data.OnPage = model.OnPage;
                this.$data.OrderBy = model.OrderBy;
                this.$data.OrderAsc = model.OrderAsc;
                this.$data.Items = model.Items;
                this.UpdatePagination(model.TotalPages, model.TotalRows);
            },
            LoadItems: function (url) {
                this.$data.Loading = true;
                var that = this;
                //that.$data.IsLoading = true; //way too fast to show loading animation, causes jerking in UI
                //console.log(url);
                axios.get(url)
                    .then(function (response) {
                        //console.log(response);
                        //console.log(response.headers.login);
                        if (response.headers.login) {
                            //Login expired -> Redirect
                            window.location.href = response.headers.login;
                        }
                        //that.$data.RequestListingModel = response.data;
                        that.$data.Items = response.data.Items;
                        that.UpdatePagination(response.data.TotalPages, response.data.TotalRows);
                        that.$data.Loading = false;
                    })
                    .catch(function (error) {
                        console.log(error);
                        //Pop up error
                        //that.$data.StatusMessage = "Load dữ liệu request không thành công. Code: " + error.response.status;
                        //that.$data.StatusTextClass = "status-danger";
                        //console.log("Failed to fetch model"); //display this somehow...
                    });
            },

            //update paging
            UpdatePagination: function (totalPages, totalRows) {
                this.$data.TotalPages = totalPages;
                this.$data.TotalRows = totalRows;
            },
            //Page number clicked handler
            PageNavClicked: function (page) {
                ////router.push({ path: `${page}/${type}/${contains}` })
                if (this.$data.Loading) return;
                this.$data.Loading = true;
                this.$data.OnPage = page;
                this.$router.push({ name: 'Index', query: this.ComposeCurrentItemsQuery(page) });

            },
            //Search button click handler
            SearchButtonClicked: function (searchModel) {
                //back to page 1 on search
                this.$data.FilterBy = searchModel.FilterBy;
                this.$data.FilterString = searchModel.FilterString;
                this.$data.OnPage = 1;
                //this will trigger route watch
                this.$router.push({ name: 'Index', query: this.ComposeCurrentItemsQuery(1) });

            },

            ComposeCurrentItemsQuery: function (pageNumber) {
                return {
                    page: pageNumber,
                    type: this.$data.FilterBy,
                    contain: this.$data.FilterString,
                    order: this.$data.OrderBy,
                    asc: this.$data.OrderAsc,
                };
            },
            //Detail
            ChildLoadCompleted: function () {
                this.$data.Loading = false;
            },
            IsShowingEventDetails: function (id) {
                var index = this.$data.ShowingEventDetails.indexOf(id);
                return index != -1;
            },
            ToggleEventDetails: function (id) {
                if (this.$data.Loading) return;
                var index = this.$data.ShowingEventDetails.indexOf(id);
                if (index == -1) {
                    //shows
                    this.HideDetails(id);
                    this.$data.ShowingEventDetails.push(id);
                    //Notify child to populate items
                    this.$data.Loading = true;
                    this.$emit("populateevents", id)
                }
                else {
                    //hides
                    this.HideDetails(id);
                }

            },
            HideDetails: function (id) {
                //hide event details
                var index = this.$data.ShowingEventDetails.indexOf(id);
                if (index != -1) {
                    this.$data.ShowingEventDetails.splice(index, 1);
                }
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
                this.$router.push({ name: 'Index', query: this.ComposeCurrentItemsQuery(1) });
            },
            OrderState: function (orderBy) {
                //console.log(orderBy);
                if (orderBy == this.$data.OrderBy) {
                    if (this.$data.OrderAsc)
                        return '&utrif;';
                    return '&dtrif;';
                }
                return '';
            }
        }
    };
</script>
<style scoped>
    .top-margin{
        margin-top: 15px;
    }
    .borderless{
        border: none !important;
    }
</style>
