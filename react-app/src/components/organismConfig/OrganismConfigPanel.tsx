import React from 'react';
import Organism from './Organism/Organism';
import ConfigForm from './ConfigForm/ConfigForm';

type OrganismConfigPanelProps = {
    organisms: Array<string>, 
    selectedOrganism: string, 
    newOrganism: string,
    organismConfigs: {}, 
}


class OrganismConfigPanel extends React.Component<any, OrganismConfigPanelProps>{
    constructor(props: OrganismConfigPanelProps) {
        super(props);
        this.state = {
            organisms: [],
            selectedOrganism: '',
            newOrganism: '',
            organismConfigs: {},
        }
    }
    
    onNameChange = (event: { target: { value: string; }; }) => {
        this.setState({newOrganism: event.target.value});
    }

    onSubmit = (event: { preventDefault: () => void; }) => {
        event.preventDefault()
        
        // Strip the name of spaces/tabs 
        const strippedName = this.state.newOrganism.replace(/\s/g,'');

        // If the stripped name contains no characters, don't add it to the list.
        if (strippedName.length === 0) {
            return;
        } 

        // If the new organism already exists in the list of organisms, don't add it to the list. 
        if (this.state.organisms.includes(this.state.newOrganism)){
            return;
        }
        
        this.setState({
            organisms: [...this.state.organisms, this.state.newOrganism],
            newOrganism: '',
        });

    }

    removeOrganism = (organismName: string) => {
        const { organisms } = this.state;
        const newOrganisms : Array<string> = [];
        organisms.forEach((organism) => {
            if (organism !== organismName) {
                newOrganisms.push(organism);
            }
        })

        this.setState({
            organisms: newOrganisms,
        })
    }

    selectOrganism = (organismName: string) => {
        this.setState({
            selectedOrganism: organismName
        });
    }

    render() {
        return (
            <div>
                <h1>This is the config Panel</h1>
                <form className="Todo" onSubmit={this.onSubmit}>
                    <input placeholder="Organism name" value={this.state.newOrganism} onChange={this.onNameChange} />
                    <button>Add</button>
                </form>
                <div>
                    {
                        this.state.organisms.map((organism:string, index:number) => (
                                <Organism key={index} organismName={organism} isSelected={organism === this.state.selectedOrganism} remove={this.removeOrganism} select={this.selectOrganism}/>
                        ))
                    }
                </div>
                <ConfigForm />
            </div>
        )
    }
}

export default OrganismConfigPanel;