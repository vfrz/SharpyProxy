import ClusterDestinationModel from "~/models/cluster/destination/ClusterDestinationModel";

export default interface ClusterModel {
    id: string,
    name: string,
    destinations: ClusterDestinationModel[],
    enabled: boolean
}