export default class UpdateClusterDestinationModel {
    public name: string = "";
    public address: string = "";
    
    constructor(name: string, address: string) {
        this.name = name;
        this.address = address;
    }
}