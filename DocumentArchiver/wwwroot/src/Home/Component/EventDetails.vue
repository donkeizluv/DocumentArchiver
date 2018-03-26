<template id="event-details-template">
    <td class="borderless" v-bind:colspan="spancol">
        <div class="row justify-content-end">
            <div class="col-12">
                <div class="card shadow-nohover">
                    <div class="card-body">
                        <div class="table-responsive">
                            <table class="table table-sm borderless">
                                <thead>
                                    <tr>
                                        <td>
                                            <button class="btn btn-link" v-on:click="OrderByClicked('EventId')" v-bind:disabled="IsLoading">
                                                <span v-html="OrderState('EventId')"></span>#
                                            </button>
                                        </td>
                                        <td>
                                            <button class="btn btn-link" v-on:click="OrderByClicked('Name')" v-bind:disabled="IsLoading">
                                                <span v-html="OrderState('Name')"></span>Sự kiện
                                            </button>
                                        </td>
                                        <td>
                                            <button class="btn btn-link" v-on:click="OrderByClicked('DateOfEvent')" v-bind:disabled="IsLoading">
                                                <span v-html="OrderState('DateOfEvent')"></span>Ngày sự kiện
                                            </button>
                                        </td>
                                        <td>
                                            <button class="btn btn-link" v-on:click="OrderByClicked('CreateTime')" v-bind:disabled="IsLoading">
                                                <span v-html="OrderState('CreateTime')"></span>Ngày tạo
                                            </button>
                                        </td>
                                        <td>
                                            <button class="btn btn-link" v-on:click="OrderByClicked('Filetype')" v-bind:disabled="IsLoading">
                                                <span v-html="OrderState('Filetype')"></span>Loại file
                                            </button>
                                        </td>
                                        <td>
                                            <button class="btn btn-link" v-on:click="OrderByClicked('Username')" v-bind:disabled="IsLoading">
                                                <span v-html="OrderState('Username')"></span>Người đăng
                                            </button>
                                        </td>
                                        <td>
                                            <span class="btn btn-link">Ghi chú</span>
                                        </td>
                                        <td>
                                            <div class="d-inline-flex">
                                                <button class="btn btn-link"
                                                        v-on:click="DownloadZip"
                                                        v-bind:disabled="!CanDownloadAll">
                                                    <span class="fas fa-download" />
                                                    Tất cả
                                                </button>
                                            </div>
                                        </td>
                                        <td>
                                            <span class="btn btn-link">Chỉnh sửa</span>
                                        </td>
                                    </tr>
                                </thead>
                                <tbody>
                                    <!--Why is this doesnt work in reverse if????-->
                                    <!--https://vuejs.org/v2/style-guide/#Avoid-v-if-with-v-for-essential-->
                                    <template v-if="HasItems">
                                        <tr v-for="event in Items" v-bind:key="event.EventId">
                                            <td class="top-border"><span>{{event.EventId}}</span></td>
                                            <!--Name-->
                                            <td v-if="IsEditMode(event.EventId)" class="top-border">
                                                <input type="text"
                                                       class="form-control form-control-sm"
                                                       v-model="event.Name"
                                                       v-bind:maxlength="FieldLength.Name" />
                                            </td>
                                            <td v-else class="top-border">
                                                <span>{{event.Name}}</span>
                                            </td>
                                            <!--Date of event-->
                                            <td v-if="IsEditMode(event.EventId)" class="top-border">
                                                <input type="date" class="form-control form-control-sm" v-model="event.DateOfEventJS" />
                                            </td>
                                            <td v-else class="top-border">
                                                <span>{{event.DateOfEventString}}</span>
                                            </td>
                                            <td class="top-border"><span>{{event.CreateTimeString}}</span></td>
                                            <td class="top-border"><span>{{event.Filetype}}</span></td>
                                            <td class="top-border"><span>{{event.Username}}</span></td>
                                            <!--Note-->
                                            <td v-if="IsEditMode(event.EventId)" class="top-border">
                                                <input type="text"
                                                       class="form-control form-control-sm"
                                                       v-model="event.Note"
                                                       v-bind:maxlength="FieldLength.Note" />
                                            </td>
                                            <td v-else class="top-border">
                                                <span>{{event.Note}}</span>
                                            </td>
                                            <!--Download file-->
                                            <td class="top-border">
                                                <button class="btn btn-sm btn-link"
                                                        v-bind:disabled="!CanDownload"
                                                        v-on:click="DownloadFile(event.EventId)">
                                                    <span class="fas fa-download" />
                                                </button>
                                            </td>
                                            <!--CRUD-->
                                            <td class="top-border">
                                                <div class="d-inline-flex">
                                                    <!--Clear changes button-->
                                                    <button v-if="IsEditMode(event.EventId)"
                                                            class="btn btn-sm btn-outline-warning"
                                                            v-on:click="ExitEditMode(event.EventId)">
                                                        <span class="fas fa-times"></span>
                                                    </button>
                                                    <!--Enter edit-->
                                                    <button v-else
                                                            v-bind:disabled="!CanUpdate"
                                                            class="btn btn-sm btn-outline-primary"
                                                            v-on:click="EnterEditMode(event.EventId)">
                                                        <span class="fas fa-pencil-alt"></span>
                                                    </button>
                                                    <!--Save changes-->
                                                    <button class="btn btn-sm mr-2 ml-2"
                                                            v-bind:class="{'btn-outline-success': CanSaveItem(event.EventId),
                                                        'btn-outline-secondary': !CanSaveItem(event.EventId)}"
                                                            v-bind:disabled="!CanSaveItem(event.EventId)"
                                                            v-on:click="SubmitChanges(event.EventId)">
                                                        <span class="fas fa-save"></span>
                                                    </button>
                                                    <!--Delete-->
                                                    <button v-if="!IsDeleteMode(event.EventId)"
                                                            class="btn btn-sm"
                                                            v-bind:disabled="!CanDelete || IsEditMode(event.EventId)"
                                                            v-bind:class="{'btn-outline-secondary': IsEditMode(event.EventId),
                                                        'btn-outline-danger': !IsEditMode(event.EventId)}"
                                                            v-on:click="EnterDeleteMode(event.EventId)">
                                                        <span class="fas fa-trash-alt"></span>
                                                    </button>
                                                    <button v-else
                                                            class="btn btn-sm btn-outline-danger"
                                                            v-on:click="DeleteEvent(event.EventId)">
                                                        <span class="fas fa-check-circle"></span>
                                                    </button>
                                                </div>
                                            </td>
                                        </tr>
                                    </template>
                                    <template v-else>
                                        <tr>
                                            <td v-bind:colspan="ColumnCount">
                                                <h6 class="text-secondary">Chưa có sự kiện nào :(</h6>
                                            </td>
                                        </tr>
                                    </template>
                                    <!--Add new record controls-->
                                    <tr>
                                        <td class="top-border"><span class="text-primary form-text">*</span></td>
                                        <td class="top-border">
                                            <input type="text"
                                                   v-bind:disabled="!CanCreate"
                                                   placeholder="Tên sự kiện"
                                                   class="form-control form-control-sm"
                                                   v-bind:class="{'attention-border' : !IsNewEventNameValid, 'green-border' : IsNewEventNameValid}"
                                                   v-model="NewItem.Name"
                                                   v-bind:maxlength="FieldLength.Name" />
                                        </td>
                                        <!--Date of event-->
                                        <td class="top-border">
                                            <input type="date"
                                                   v-bind:disabled="!CanCreate"
                                                   class="form-control form-control-sm"
                                                   v-bind:class="{'attention-border' : !IsNewEventDateValid, 'green-border' : IsNewEventDateValid}"
                                                   v-model="NewItem.DateOfEvent" />
                                        </td>
                                        <!--File upload-->
                                        <td class="top-border" colspan="2">
                                            <uploader v-bind:disabled="!CanCreate"
                                                      v-bind:accept="UploaderOption.Accept"
                                                      v-bind:max-size="UploaderOption.MaxSize"
                                                      v-bind:button-text="UploaderOption.Text"
                                                      ref="uploader">
                                            </uploader>
                                        </td>
                                        <!--Note-->
                                        <td class="top-border" colspan="3">
                                            <input type="text"
                                                   v-bind:disabled="!CanCreate"
                                                   placeholder="Ghi chú"
                                                   v-model="NewItem.Note"
                                                   class="form-control form-control-sm"
                                                   v-bind:maxlength="FieldLength.Note" />
                                        </td>
                                        <!--CRUD controls-->
                                        <td class="top-border">
                                            <button v-on:click="PostNewItem"
                                                    v-bind:disabled="!CanSaveNewItem"
                                                    class="btn btn-sm"
                                                    v-bind:class="{'btn-success' : CanSaveNewItem,
                                                    'btn-secondary' : !CanSaveNewItem}">
                                                <i v-if="IsUploading" class="fas fa-spinner fa-pulse" />
                                                <i v-else>OK</i>
                                            </button>
                                        </td>
                                    </tr>
                                </tbody>
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
    import axios from 'axios'
    import pagenav from 'vuejs-paginate'
    import queryBuilder from 'query-string'

    import uploader from '../Component/Uploader.vue'
    import moment from 'moment'
    import appConst from '../AppConst'

    const JSDate = 'YYYY-MM-DD';
    const successEvent = 'success';
    const exEvent = 'exception';

    //Event names
    const startUploadEventName = 'startuploading';
    const uploadFinishedEventName = 'uploadfinished';
    const populateEventBroadcast = 'populatedetails';
    const populateCompleted = 'populatecompleted';


    export default {
        name: 'event-details',
        template: '#event-details-template',
        components: {
            'page-nav': pagenav,
            'uploader': uploader
        },
        props: {
            id: Number,
            spancol: {
                type: Number,
                default: 7
            }
        },
        data: function () {
            return {
                Loading: false,
                Populated: false,
                Uploading: false,

                OnPage: 1,
                OrderBy: 'EventId',
                OrderAsc: true,

                NewItem: {
                    Name: null,
                    //DateOfEvent: moment().format(JSDate),
                    DateOfEvent: null,
                    Note: null
                },
                FieldLength: {
                    Name: 50,
                    Note: 150,
                },
                ColumnCount: 9,
                Items: [],
                ItemsCopy: [], //Store original item to support revert
                TotalRows: 0,
                TotalPages: 0,

                UploaderOption: {
                    Text: "Upload (13mb<)",
                    //Accept: '.jpg, .jpeg, .bmp, .png, .doc, .docx, .msg, .pdf',
                    //MaxSize: 3145728 //3 MB
                    Accept: ".zip, .rar, .7z, .tar, .gzip",
                    MaxSize: 13631488 //13MB
                }
            };
        },
        computed: {
            IsUploading: function () {
                return this.Uploading;
            },
            IsLoading: function () {
                return this.Loading;
            },
            IsNewEventDateValid: function () {
                return this.IsDateValid(this.NewItem.DateOfEvent);
            },
            IsNewEventFileValid: function () {
                return this.$refs.uploader.IsFileValid;
            },
            IsNewEventNameValid: function () {
                if (this.NewItem.Name)
                    return true;
                return false;
            },
            HasItems: function () {
                return this.Items.length > 0;
            },
            CanUpdate: function () {
                return this.$store.getters.CanUpdate;
            },
            CanDelete: function () {
                return this.$store.getters.CanDelete;
            },
            CanCreate: function () {
                return this.$store.getters.CanCreate;
            },
            CanDownload: function () {
                return this.$store.getters.CanDownload;
            },
            CanDownloadAll: function () {
                if (this.HasItems)
                    if (this.CanDownload)
                        return true;
                return false;
            },
            CanSaveNewItem: function () {
                if (this.IsNewEventNameValid &&
                    this.IsNewEventDateValid &&
                    this.IsNewEventFileValid)
                    return true;
                return false;
            },
            //Only show page nav if total pages > 1
            CanShowPageNav: function () {
                return this.TotalPages > 1;
            },
            GetCurrentItemsAPI: function () {
                return appConst.EventListingAPI + queryBuilder.stringify(this.ComposeCurrentItemsQuery(this.OnPage));
            }
        },
        created: function () {
            //Listen to parent's event
            this.$parent.$on(populateEventBroadcast, this.Populate);
        },
        methods: {
            ComposeCurrentItemsQuery: function (pageNumber) {
                return {
                    id: this.id,
                    page: pageNumber,
                    order: this.OrderBy,
                    asc: this.OrderAsc,
                };
            },
            Populate: function (id) {
                //Check if this component being clicked
                if (id != this.id) return;
                //Check if has items
                if (this.Populated) {
                    //Do nothing
                    this.$emit(populateCompleted);
                    return;
                }
                //Load items
                this.LoadItems(this.GetCurrentItemsAPI);
            },
            LoadItems: function (url) {
                //console.log(url);
                this.Loading = true;
                var that = this;

                axios.get(url)
                    .then(function (response) {
                        that.Items = response.data.Items;
                        that.RefreshCopy(); //Clone Items to arr
                        that.UpdatePagination(response.data.TotalPages, response.data.TotalRows);
                        that.Loading = false;
                        that.Populated = true;
                        that.$emit(populateCompleted);
                    })
                    .catch(function (error) {
                        console.log(error);
                        that.$emit(exEvent, error);
                    });
            },

            PostNewItem: function () {
                if (this.IsLoading || this.IsUploading) return;
                this.$emit(startUploadEventName);
                this.Uploading = true;
                this.Loading = true;
                var url = appConst.NewEventAPI;
                var that = this;
                var formData = new FormData();
                formData.append('ContractId', this.id);
                formData.append('Name', this.NewItem.Name);
                formData.append('DateOfEvent', this.NewItem.DateOfEvent);
                formData.append('File', this.$refs.uploader.GetFile);
                formData.append('Note', this.NewItem.Note);
                //console.log(formData);
                //var axiosConfig = {
                //    //Progress changed callback
                //    onUploadProgress: function (progressEvent) {
                //        var percent = progressEvent.loaded / progressEvent.total * 100;
                //        console.log(percent);
                //        that.setProgressBar(percent);
                //    }
                //}
                //this.startProgressBar();
                axios.post(url, formData, axiosConfig)
                    .then(function (response) {
                        that.$emit(successEvent, 'Thêm sự kiện mới thành công!')
                        that.ClearNewItem();
                        that.Refresh();
                        that.$emit(uploadFinishedEventName);
                        that.Uploading = false;
                        that.Loading = false;
                        //that.finishProgressBar();
                    })
                    .catch(function (error) {
                        //Not 2xx code
                        console.log(error);
                        that.$emit(uploadFinishedEventName);
                        that.$emit(exEvent, 'Lỗi trong quá trình tạo sự kiện mới.');
                        that.Uploading = false;
                        that.Loading = false;
                        //that.failProgressBar();
                    });
            },
            SubmitChanges: function (id) {
                var url = appConst.UpdateEventAPI;
                var that = this;
                var itemIndex = this.FindItemIndex(id)
                var formData = new FormData();
                formData.append('EventId', id);
                formData.append('Name', this.Items[itemIndex].Name);
                formData.append('DateOfEvent', this.Items[itemIndex].DateOfEventJS);
                formData.append('Note', this.Items[itemIndex].Note);
                //console.log(formData);

                axios.post(url, formData)
                    .then(function (response) {
                        if (response.headers.login) {
                            //Login expired -> Redirect
                            window.location.href = response.headers.login;
                            return;
                        }
                        that.$emit(successEvent, 'Cập nhật thành công!');
                        that.$set(that.Items[itemIndex], 'EditMode', false)
                        that.Refresh();
                    })
                    .catch(function (error) {
                        console.log(error);
                        that.$emit(exEvent, 'Lỗi trong quá trình cập nhật sự kiên.');
                    });
            },
            DeleteEvent: function (id) {
                var url = appConst.DeleteEventAPI;
                var that = this;
                var itemIndex = this.FindItemIndex(id)
                var formData = new FormData();
                formData.append('eventId', id);
                //console.log(formData);

                axios.post(url, formData)
                    .then(function (response) {
                        that.$emit(successEvent, 'Xóa thành công!');
                        that.Refresh();
                    })
                    .catch(function (error) {
                        console.log(error);
                        that.$emit(exEvent, 'Lỗi trong quá trình xóa sự kiên.');
                    });
            },
            CanSaveItem(id) {
                var index = this.FindItemIndex(id);
                if (index == -1) {
                    console.log('CanSaveItem: Cant find ' + id);
                    return false;
                }
                //Must be in Edit mode to save
                if (!this.IsEditMode(id)) return false;
                //Values check
                if (this.Items[index].Name &&
                    this.IsDateValid(this.Items[index].DateOfEventJS))
                    return true;
                return false;
            },
            RefreshCopy: function () {
                var i = this.Items.length;
                //No better way to clone?
                while (i--) this.ItemsCopy[i] = JSON.parse(JSON.stringify(this.Items[i]));
            },
            ExitEditMode: function (id) {
                var index = this.FindItemIndex(id);
                if (index == -1) {
                    console.log('ExitEditMode: Cant find ' + id);
                    return;
                }
                //No better way to clone?
                var revert = JSON.parse(JSON.stringify(this.ItemsCopy[index]));
                //console.log(clone);
                this.$set(this.Items, index, revert);
            },
            //Values changed tracking
            //Edit mode
            EnterEditMode: function (id) {
                var index = this.FindItemIndex(id);
                if (index == -1) {
                    return;
                }
                //Exit delete mode if needed
                this.$set(this.Items[index], 'DeletePromt', false)
                this.$set(this.Items[index], 'EditMode', true)
            },
            IsEditMode: function (id) {
                var index = this.FindItemIndex(id);
                if (index == -1) {
                    return;
                }
                if (this.Items[index].EditMode) {
                    return true;
                }
                return false;
            },
            //Delete mode
            EnterDeleteMode: function (id) {
                var index = this.FindItemIndex(id);
                if (index == -1) {
                    return;
                }
                this.$set(this.Items[index], 'DeletePromt', true)
            },
            IsDeleteMode: function (id) {
                var index = this.FindItemIndex(id);
                if (index == -1) {
                    return;
                }
                if (this.Items[index].DeletePromt) {
                    return true;
                }
                return false;
            },
            //Clear new item
            ClearNewItem: function () {
                this.$refs.uploader.Clear();
                this.NewItem.Name = null;
                this.NewItem.DateOfEvent = null;
                this.NewItem.Note = null;
            },
            //Item index stuff
            FindItemIndex: function (id) {
                return this.Items.findIndex(x => x.EventId == id);
            },
            IsDateValid(dateString) {
                return moment(dateString, JSDate, true).isValid();
            },
            DownloadFile: function (id) {
                var s64 = encodeURIComponent(btoa(id));
                var url = appConst.DownloadAPI;
                url = url.replace('{id}', s64);
                console.log(s64);
                console.log(url);
                window.open(url);
            },
            DownloadZip: function () {
                var s64 = encodeURIComponent(btoa(this.id));
                var url = appConst.DownloadZipAPI;
                url = url.replace('{id}', s64);
                console.log(s64);
                console.log(url);
                window.open(url);
            },
            //order methods
            OrderByClicked: function (orderBy) {
                if (this.Loading) return;
                this.Loading = true; //prevent click spamming
                //Flip order by
                if (this.OrderBy == orderBy) {
                    this.OrderAsc = !this.OrderAsc;
                }
                else {
                    //Order this column
                    this.OrderBy = orderBy;
                    this.OrderAsc = true;
                }
                this.LoadItems(this.GetCurrentItemsAPI);
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
            PageNavClicked: function (page) {
                if (this.Loading) return;
                this.Loading = true;
                this.OnPage = page;
                this.LoadItems(this.GetCurrentItemsAPI);
            },
            UpdatePagination: function (totalPages, totalRows) {
                this.TotalPages = totalPages;
                this.TotalRows = totalRows;
            },
        }
    }
</script>
<style scoped>
    table.borderless td {
        border: none;
    }
    .attention-border {
        border-color: #ffc107;
    }
    .green-border {
        border-color: #28a745;
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