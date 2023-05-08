import CreateClusterDestinationModel from "~/models/cluster/destination/CreateClusterDestinationModel";

export default interface CreateClusterModel {
    id: string,
    destinations: CreateClusterDestinationModel[]
}