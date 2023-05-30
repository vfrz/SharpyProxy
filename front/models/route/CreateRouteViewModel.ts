import ClusterModel from "~/models/cluster/ClusterModel";

export default interface CreateRouteViewModel {
    name: string,
    enabled: boolean,
    matchPath: string,
    matchHosts: string,
    cluster?: ClusterModel
}