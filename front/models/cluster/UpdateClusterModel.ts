import UpdateClusterDestinationModel from "~/models/cluster/destination/UpdateClusterDestinationModel";

export default class UpdateClusterModel {
    public id: string = "";
    public name: string = "";
    public destinations: UpdateClusterDestinationModel[] = [];
    public enabled: boolean = true;
}