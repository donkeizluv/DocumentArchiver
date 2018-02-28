﻿export default {
    //Claims
    Ability: 'Ability',
    LayerName: 'LayerName',
    LayerRank: 'LayerRank',
    CanDeleteClaim : 'Delete',
    CanUpdateClaim : 'Update',
    CanCreateClaim: 'Create',
    CanDownloadClaim: 'Download',

    //API

    //Contract
    ContractListingAPI: "/API/Contract/Get?",
    CheckContractAPI: "/API/Contract/Check",
    CreateContractAPI: "/API/Contract/Create",

    //Event
    EventListingAPI: "/API/Event/Get?",
    NewEventAPI: "/API/Event/Create",
    UpdateEventAPI: "/API/Event/Update",
    DeleteEventAPI: "/API/Event/Delete",
    DownloadAPI: "/API/Event/Download?id={id}",
    DownloadZipAPI: "/API/Event/DownloadZip?id={id}"
}
