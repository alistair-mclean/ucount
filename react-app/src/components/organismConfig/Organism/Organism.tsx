import React from 'react';
import './Organism.css';
import { async } from 'q';

type OrganismProps = {
    organismName: string,
    isSelected: boolean,
    remove: void | null,
    select: void | null,
    unselect: void | null,
}
export default class Organism extends React.Component<any, OrganismProps> {
    constructor(props:OrganismProps) {
        super(props);
        if (props) {
            const { organismName, isSelected, remove, select, unselect } = this.props;
            this.state = {
                organismName,
                isSelected,
                remove,
                select,
                unselect
            };
        } else {
            this.state = {
                organismName: '',
                isSelected: false,
                remove: null,
                select: null,
                unselect: null,
            };
        }
    }

    removeOrganism = () => {
        // TODO - Figure out how to call these functions properly from state
        this.props.remove(this.state.organismName);
    }

    selectOrganism = () => {
        this.props.select(this.state.organismName);
        this.setState({
            isSelected: true
        })
    }

    unselectOrganism = () => {
        this.setState({
            isSelected: false,
        });
    }
    
    render () {
        const { isSelected } = this.props;
        return (
            <div>
                {isSelected ? (
                    <div className='organism-selected'  onMouseDown={this.selectOrganism}>
                    {this.props.organismName}<button className='remove' onClick={this.removeOrganism}>-</button>
                </div>
                ) : (
                <div className='organism'  onMouseDown={this.selectOrganism}>
                    {this.props.organismName}<button className='remove' onClick={this.removeOrganism}>-</button>
                </div>
                )}
            </div>
        )
    }
}