<template id="listing-template">
    <div>
        <!--Modals-->
            <!--<modal name="m-app-error" v-bind:clickToClose=false>
        </modal>-->
        <v-dialog :clickToClose=false />
        <!--End modals-->
        <div class="row top-margin">
            <div class="col-sm-8 mx-auto">
                <search-bar v-bind:isdisabled="IsLoading"
                            v-bind:value-pairs="SearchBarValues"
                            v-on:submit="SearchButtonClicked"></search-bar>
            </div>
        </div>
        <div class="row top-margin">
            <div class="col-12">
                <div class="table-responsive text-center">
                    <table class="table table-hover">
                        <thead>
                            <tr>
                                <th>
                                    <button class="btn btn-link" v-on:click="CollapseAll" v-bind:disabled="IsLoading">
                                        <span class="fas fa-minus fa-sm"></span> All
                                    </button>
                                </th>
                                <th>
                                    <button class="btn btn-link" v-on:click="OrderByClicked('ContractNumber')" v-bind:disabled="IsLoading">
                                        <span v-html="OrderState('ContractNumber')"></span>Số Hd.
                                    </button>
                                </th>
                                <th>
                                    <button class="btn btn-link" v-on:click="OrderByClicked('CustomerName')" v-bind:disabled="IsLoading">
                                        <span v-html="OrderState('CustomerName')"></span>Tên Kh.
                                    </button>
                                </th>
                                <th>
                                    <button class="btn btn-link" v-on:click="OrderByClicked('IdentityCard')" v-bind:disabled="IsLoading">
                                        <span v-html="OrderState('IdentityCard')"></span>CMND
                                    </button>
                                </th>
                                <th>
                                    <button class="btn btn-link" v-on:click="OrderByClicked('Phone')" v-bind:disabled="IsLoading">
                                        <span v-html="OrderState('Phone')"></span>SĐT
                                    </button>
                                </th>
                                <th>
                                    <button class="btn btn-link" v-on:click="OrderByClicked('CreateTime')" v-bind:disabled="IsLoading">
                                        <span v-html="OrderState('CreateTime')"></span>Ngày tạo
                                    </button>
                                </th>
                                <th>
                                    <button class="btn btn-link" v-on:click="OrderByClicked('Username')" v-bind:disabled="IsLoading">
                                        <span v-html="OrderState('Username')"></span>Người tạo
                                    </button>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <template v-for="item in Items">
                                <tr>
                                    <td>
                                        <button v-on:click="ToggleEventDetails(item.ContractId)" class="btn btn-sm btn-link" v-bind:disabled="IsLoading">
                                            <span class="fas" v-bind:class="{'fa-minus': IsShowingEventDetails(item.ContractId),
                                                  'fa-plus': !IsShowingEventDetails(item.ContractId)}">
                                            </span>
                                        </button>
                                    </td>
                                    <td>
                                        <span>{{item.ContractNumber}}</span>
                                    </td>
                                    <td>
                                        <span>{{item.CustomerName}}</span>
                                    </td>
                                    <td>
                                        <span>{{item.IdentityCard}}</span>
                                    </td>
                                    <td>
                                        <span>{{item.Phone}}</span>
                                    </td>
                                    <td>
                                        <span>{{item.CreateTimeString}}</span>
                                    </td>
                                    <td>
                                        <span>{{item.Username}}</span>
                                    </td>
                                </tr>
                                <!--@*expandable details*@-->
                                <tr>
                                    <!--colspan 9999 will break Edge-->
                                    <event-details v-show="IsShowingEventDetails(item.ContractId)"
                                                   v-bind:key="item.ContractId"
                                                   v-bind:id="item.ContractId"
                                                   v-bind:spancol=7
                                                   v-on:populatecompleted="SetLoadingState(false)"
                                                   v-on:exception="ShowErrorDialog"
                                                   v-on:success="ShowSuccessToast"
                                                   v-on:startuploading="SetLoadingState(true)"
                                                   v-on:uploadfinished="SetLoadingState(false)"/>
                                <!--any better ways to do this?-->
                                </tr>
                            </template>
                            <!--New case-->
                            <tr>
                                <td><span class="text-primary form-text">*</span></td>
                                <!--Contract number-->
                                <td>
                                    <div class="input-group">
                                        <input class="form-control form-control-sm"
                                               placeholder="Số hợp đồng"
                                               type="search"
                                               v-bind:disabled="!CanCreate"
                                               v-model="NewItem.ContractNumber"
                                               maxlength="20">
                                        <span class="input-group-btn">
                                            <button class="btn btn-link"
                                                    v-on:click="CheckContract"
                                                    v-bind:disabled="!CanCheck">
                                                <span class="fas fa-check"></span>Check
                                            </button>
                                        </span>
                                    </div>
                                </td>
                                <!--Cus name-->
                                <td>
                                    <input type="text"
                                           class="form-control form-control-sm"
                                           placeholder="Tên khách hàng"
                                           readonly
                                           v-model="NewItem.CustomerName" />
                                </td>
                                <!--Id card-->
                                <td>
                                    <input type="text"
                                           class="form-control form-control-sm"
                                           placeholder="CMND"
                                           readonly
                                           v-model="NewItem.IdentityCard" />
                                </td>
                                <!--Phone-->
                                <td>
                                    <input type="text"
                                           class="form-control form-control-sm"
                                           placeholder="Số điện thoại"
                                           readonly
                                           v-model="NewItem.Phone" />
                                </td>
                                <!--Operations buttons-->
                                <td colspan="2">
                                    <div class="clearfix">
                                        <button class="btn btn-sm float-left"
                                                v-bind:class="{'btn-success' : IsNewCaseValid,
                                                'btn-secondary' : !IsNewCaseValid}"
                                                v-bind:disabled="!IsNewCaseValid"
                                                v-on:click="PostNewItem">
                                            OK
                                        </button>
                                    </div>

                                    <!--<div class="d-inline-flex">
                                    <button class="btn btn-sm btn-outline-danger ml-2"
                                            v-on:click="ClearNewItem">
                                        <span class="fas fa-times"></span>
                                    </button>
                                </div>-->
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <div class="row top-margin">
            <div class="col-lg-6 mx-auto">
                <page-nav :page-count="TotalPages"
                          :click-handler="PageNavClicked"
                          ref="casepaginate"
                          :page-range="2"
                          :prev-text="'Trước'"
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
</template>
<script>
    import axios from 'axios'
    import common from '../Common'
    import searchBar from './SearchBar.vue'
    import eventDetails from './EventDetails.vue'
    import pagenav from 'vuejs-paginate'
    import queryBuilder from 'query-string'
    import appConst from '../AppConst'

    import { SET_ABILITY, SET_LAYER, SET_LAYERRANK } from '../actions'
    const errorModal = 'm-app-error';
    //Events
    const PopulateDetail = 'populatedetails';

    //Message
    const CheckContractOK = 'Số hợp đồng hợp lệ';
    const NewItemCreated = 'Tạo thành công!';
    const NewItemFailed = 'Tạo thất bại!';
    const GeneralError = 'Có lỗi xãy ra :(';

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
        //mounted: function () {
        //    //this.Init();
        //},
        created: function () {
            this.Init();
        },
        watch: {
            '$route'(to, from) {
                var page = to.query.page || 1;
                if (page < 1) page = 1;
                this.OnPage = page;
                this.FilterBy = to.query.type || '',
                this.FilterString = to.query.contain || '',
                this.OrderBy = to.query.order || '',
                this.OrderAsc = to.query.asc || true,
                this.LoadItems(this.GetCurrentItemsAPI);
                //Sync page number with nav control
                //0 based
                this.$refs.casepaginate.selected = this.OnPage - 1;
            },
            '$data.NewItem.ContractNumber'(to, from) {
                //Clear values when ContractNumber changes
                this.NewItem.CustomerName = null;
                this.NewItem.Phone = null;
                this.NewItem.IdentityCard = null;
                this.IsNewCaseValid = false;
            },
        },
        data: function () {
            return {
                Loading: false,

                OnPage: 1,
                FilterBy: '',
                FilterString: '',
                OrderBy: '',
                OrderAsc: true,
                Items: [],
                TotalRows: 0,
                TotalPages: 0,

                //Details
                ShowingEventDetails: [],

                //Crud
                IsNewCaseValid: false,
                NewItem: {
                    ContractNumber: null,
                    CustomerName: null,
                    IdentityCard: null,
                    Phone: null
                },
                //Search bar value pairs
                SearchBarValues: appConst.SearchBarValues
            };
        },
        computed: {
            CanCreate: function () {
                return this.$store.getters.CanCreate;
            },
            GetCurrentItemsAPI: function () {
                return appConst.ContractListingAPI + queryBuilder.stringify(this.ComposeCurrentItemsQuery(this.OnPage));
            },
            CanSaveNewItem: function () {
                return this.IsNewCaseValid;
            },
            CanCheck: function () {
                //No special chars in contract number
                var format = /[ !@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/;
                if (this.NewItem.ContractNumber) {
                    if (!format.test(this.NewItem.ContractNumber))
                        return true;
                }
                return false;
            },
            IsLoading: function () {
                return this.Loading;
            }
        },
        methods: {
            //init app
            Init: function () {
                var router = this.$router;
                var page = router.history.current.query.page || 1;
                if (page < 1) page = 1;
                var type = router.history.current.query.type || '';
                var contain = router.history.current.query.contain || '';
                var order = router.history.current.query.order || '';
                var asc = router.history.current.query.asc || true;

                this.FilterBy = type;
                this.FilterString = contain;
                this.OnPage = page;
                this.OrderBy = order;
                this.OrderAsc = asc;
                this.Refresh();
            },
            LoadItems: function (url) {
                this.SetLoadingState(true);
                var that = this;
                //that.$data.IsLoading = true; //way too fast to show loading animation, causes jerking in UI
                //console.log(url);
                axios.get(url)
                    .then(function (response) {
                        //that.$data.RequestListingModel = response.data;
                        that.Items = response.data.Items;
                        that.UpdatePagination(response.data.TotalPages, response.data.TotalRows);
                        that.SetLoadingState(false);
                        that.ShowingEventDetails = []; //Reset shown details
                        //Re-render
                        //that.$forceUpdate();
                    })
                    .catch(function (error) {
                        console.log(error);
                        that.ShowErrorDialog(GeneralError);
                    });
            },
            ShowSuccessToast(mess) {
                //This has shitty support for specific icon & multiple style class
                this.$toasted.success(mess, {
                    icon: 'fa-check-circle',
                    className: 'toast-font-size'});
            },
            ShowInfoToast(mess) {
                this.$toasted.info(mess, {
                    icon: 'fa-exclamation-circle',
                    className: 'toast-font-size'
                });
            },
            ShowErrorDialog(mess) {
                this.$modal.show('dialog', {  
                    title: 'Lỗi :(',
                    text: mess,
                    buttons: [
                        {
                            title: 'Đóng',
                            handler: () => { this.$modal.hide('dialog') }
                        }
                    ]});
            },
            //update paging
            UpdatePagination: function (totalPages, totalRows) {
                this.TotalPages = totalPages;
                this.TotalRows = totalRows;
            },
            //Page number clicked handler
            PageNavClicked: function (page) {
                ////router.push({ path: `${page}/${type}/${contains}` })
                if (this.IsLoading) return;
                this.SetLoadingState(true);
                this.OnPage = page;
                this.$router.push({ name: 'Home', query: this.ComposeCurrentItemsQuery(page) });
            },
            //Search button click handler
            SearchButtonClicked: function (searchModel) {
                //back to page 1 on search
                this.FilterBy = searchModel.FilterBy;
                this.FilterString = searchModel.FilterString;
                this.OnPage = 1;
                //this will trigger route watch
                this.$router.push({ name: 'Home', query: this.ComposeCurrentItemsQuery(1) });

            },

            ComposeCurrentItemsQuery: function (pageNumber) {
                return {
                    page: pageNumber,
                    type: this.FilterBy,
                    contain: this.FilterString,
                    order: this.OrderBy,
                    asc: this.OrderAsc,
                };
            },
            //CRUD
            ClearNewItem: function () {
                this.NewItem.ContractNumber = null;
                this.NewItem.CustomerName = null;
                this.NewItem.Phone = null;
                this.NewItem.IdentityCard = null;
                this.IsNewCaseValid = false;
            },
            //Check before submit new item
            CheckContract: function () {
                var url = appConst.CheckContractAPI;
                var that = this;
                var formData = new FormData();
                formData.append('contractNumber', this.NewItem.ContractNumber);
                //console.log(formData);

                axios.post(url, formData)
                    .then(function (response) {
                        //console.log(response);
                        if (response.data.Valid) {
                            //Check OK
                            that.ShowSuccessToast(CheckContractOK);
                            that.IsNewCaseValid = true;
                            that.NewItem = response.data.Data;
                        }
                        else {
                            //Not OK
                            that.ShowInfoToast(response.data.Message);
                        }
                    })
                    .catch(function (error) { //Not 2xx code
                        that.ShowErrorDialog(GeneralError);
                    });
            },
            //Call api to create new item
            PostNewItem: function () {
                var url = appConst.CreateContractAPI;
                var that = this;
                var formData = new FormData();
                formData.append('contractNumber', this.NewItem.ContractNumber);
                //console.log(formData);

                axios.post(url, formData)
                    .then(function (response) {
                        if (response.data.Valid) {
                            that.ShowSuccessToast(NewItemCreated);
                            that.ClearNewItem();
                            that.Refresh();
                        }
                        else {
                            that.ShowInfoToast(response.data.Message);
                        }
                    })
                    .catch(function (error) {
                        that.ShowErrorDialog(NewItemFailed);
                    });
            },
            //Detail
            //DetailsLoadCompleted: function () {
            //    this.IsLoading = false;
            //},
            IsShowingEventDetails: function (id) {
                var index = this.ShowingEventDetails.indexOf(id);
                if (index != -1) {
                    return true;
                }
                return false;
            },
            ToggleEventDetails: function (id) {
                if (this.IsLoading) return;
                var index = this.ShowingEventDetails.indexOf(id);
                if (index == -1) {
                    //shows
                    this.HideDetails(id);
                    this.ShowingEventDetails.push(id);
                    this.SetLoadingState(true);
                    //Broadcast events
                    this.$emit(PopulateDetail, id);
                }
                else {
                    //hides
                    this.HideDetails(id);
                }

            },
            HideDetails: function (id) {
                //hide event details
                var index = this.ShowingEventDetails.indexOf(id);
                if (index != -1) {
                    this.ShowingEventDetails.splice(index, 1);
                }
            },
            CollapseAll: function () {
                while (this.ShowingEventDetails.length > 0) { this.ShowingEventDetails.pop(); }
            },
            //order methods
            OrderByClicked: function (orderBy) {
                if (this.IsLoading) return;
                this.SetLoadingState(true); //prevent click spamming
                //Flip order by
                if (this.OrderBy == orderBy) {
                    this.OrderAsc = !this.OrderAsc;
                }
                else {
                    //Order this column
                    this.OrderBy = orderBy;
                    this.OrderAsc = true;
                }
                this.$router.push({ name: 'Home', query: this.ComposeCurrentItemsQuery(1) });
            },
            OrderState: function (orderBy) {
                //console.log(orderBy);
                if (orderBy == this.OrderBy) {
                    if (this.OrderAsc)
                        return '&utrif;';
                    return '&dtrif;';
                }
                return '';
            },
            Refresh: function () {
                this.LoadItems(this.GetCurrentItemsAPI);
            },
            SetLoadingState(state) {
                this.Loading = state;
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
