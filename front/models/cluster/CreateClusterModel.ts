import CreateClusterDestinationModel from "~/models/cluster/destination/CreateClusterDestinationModel";

export default interface CreateClusterModel {
    name: string,
    destinations: CreateClusterDestinationModel[],
    enabled: boolean
}