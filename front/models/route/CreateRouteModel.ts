export default interface CreateRouteModel {
    name: string,
    enabled: boolean,
    matchPath?: string,
    matchHosts?: string[],
    clusterId: string
}