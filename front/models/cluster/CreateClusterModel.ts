import CreateClusterDestinationModel from "~/models/cluster/destination/CreateClusterDestinationModel";

export default class CreateClusterModel {
    public id: string = "";
    public destinations: CreateClusterDestinationModel[] = [];
    public enabled: boolean = true;
}