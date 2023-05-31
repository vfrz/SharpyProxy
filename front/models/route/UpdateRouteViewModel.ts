import ClusterModel from "~/models/cluster/ClusterModel";

export default interface UpdateRouteViewModel {
    name: string,
    enabled: boolean,
    matchPath: string,
    matchHosts: string,
    cluster?: ClusterModel
}